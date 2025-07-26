using ModelContextProtocol.Server;
using System.ComponentModel;

namespace FrameworkAssistant.Tools.Test;

[McpServerToolType]
public class TimeTool
{
    [McpServerTool, Description("Gets the current date and time.")]
    public string GetCurrentTime() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
}
