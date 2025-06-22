using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using ModelContextProtocol.Server;

namespace sqlserver_mcp;

[McpServerToolType]
public static class SqlServerTool
{
    // Helper method to create connection string
    private static string BuildConnectionString(string server, string database, string userId, string password)
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = server,
            InitialCatalog = database,
            UserID = userId,
            Password = password,
            TrustServerCertificate = true,
            Encrypt = false // 필요 시 true
        };
        return builder.ConnectionString;
    }

    [McpServerTool, Description("Get all table names in the specified database.")]
    public static List<string> GetTableList(string server, string database, string userId, string password)
    {
        var connectionString = BuildConnectionString(server, database, userId, password);
        var tables = new List<string>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT TABLE_SCHEMA + '.' + TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
            }
        }

        return tables;
    }

    [McpServerTool, Description("Get all column names and types for a specific table.")]
    public static List<string> GetColumnList(string server, string database, string userId, string password, string tableSchema, string tableName)
    {
        var connectionString = BuildConnectionString(server, database, userId, password);
        var columns = new List<string>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"
                SELECT COLUMN_NAME, DATA_TYPE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = @TableSchema AND TABLE_NAME = @TableName";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TableSchema", tableSchema);
                command.Parameters.AddWithValue("@TableName", tableName);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var columnInfo = $"{reader.GetString(0)} ({reader.GetString(1)})";
                        columns.Add(columnInfo);
                    }
                }
            }
        }

        return columns;
    }

    [McpServerTool, Description("Execute a query and return the result as a list of JSON strings.")]
    public static List<string> ExecuteQuery(string server, string database, string userId, string password, string query)
    {
        var connectionString = BuildConnectionString(server, database, userId, password);
        var results = new List<string>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();

            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            var schemaTable = reader.GetSchemaTable();

            if (schemaTable == null || schemaTable.Rows == null || schemaTable.Rows.Count == 0)
            {
                return results; // 컬럼이 없으면 빈 리스트 반환
            }

            var columnNames = schemaTable.Rows
                .Cast<DataRow>()
                .Select(row => row.Field<string>("ColumnName")!)
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList();

            while (reader.Read())
            {
                var row = new Dictionary<string, object?>();

                foreach (var columnName in columnNames)
                {
                    var value = reader[columnName];
                    row[columnName] = value == DBNull.Value ? null : value;
                }

                var json = System.Text.Json.JsonSerializer.Serialize(row);
                results.Add(json);
            }
        }

        return results;
    }

}
