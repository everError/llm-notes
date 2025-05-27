using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using SemanticKernelAgent.Models;

namespace SemanticKernelAgent.Controllers;

[ApiController]
[Route("agent")]
public class AgentController : ControllerBase
{
    private readonly Kernel _kernel;

    public AgentController(Kernel kernel)
    {
        _kernel = kernel;
    }

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

        var result = await _kernel.InvokePromptAsync(prompt);
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

        Response.Headers["Content-Type"] = "text/event-stream";
        Response.Headers["Cache-Control"] = "no-cache";
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
