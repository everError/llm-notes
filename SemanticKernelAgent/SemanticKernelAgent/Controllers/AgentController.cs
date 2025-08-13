using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticKernelAgent.Models;

namespace SemanticKernelAgent.Controllers;

[ApiController]
[Route("agent")]
public class AgentController(Kernel kernel) : ControllerBase
{
    private readonly Kernel _kernel = kernel;

    [HttpPost("ask")]
    public async Task<IActionResult> AskAsync([FromBody] QuestionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest("질문이 비어있습니다.");

        var prompt = $"""
        당신은 전문적인 인공지능 비서입니다. 사용자의 질문에 대해 정확하고 깊이 있는 정보를 제공하세요.

        질문:
        {request.Question}

        답변:
        """;
        // Enable automatic function calling
#pragma warning disable SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.
        OpenAIPromptExecutionSettings executionSettings = new()
        {
            Temperature = 0,
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(options: new() { RetainArgumentTypes = true })
        };
#pragma warning restore SKEXP0001 // 형식은 평가 목적으로 제공되며, 이후 업데이트에서 변경되거나 제거될 수 있습니다. 계속하려면 이 진단을 표시하지 않습니다.
        var result = await _kernel.InvokePromptAsync(prompt, new(executionSettings));
        return Ok(result.ToString());
    }
    [HttpPost("ask-sse")]
    public async Task AskSseAsync([FromBody] QuestionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Question))
        {
            Response.StatusCode = 400;
            await Response.WriteAsync("질문이 비어있습니다.");
            return;
        }

        var prompt = $"""
        당신은 전문적인 인공지능 비서입니다. 사용자의 질문에 대해 정확하고 깊이 있는 정보를 제공하세요.

        질문:
        {request.Question}

        답변:
        """;

        Response.Headers.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers["X-Accel-Buffering"] = "no"; // nginx 버퍼링 방지
        Response.StatusCode = 200;

        await foreach (var chunk in _kernel.InvokePromptStreamingAsync(prompt))
        {
            var content = chunk.ToString();
            if (!string.IsNullOrWhiteSpace(content))
            {
                var sseFormatted = $"data: {content.Replace("\n", "\\n")}\n\n"; // sse 형식 포맷
                await Response.WriteAsync(sseFormatted);
                await Response.Body.FlushAsync(); // 즉시 전송
            }
        }
    }

}
