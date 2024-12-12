using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswerController(IAnswerService answerService) : ControllerBase
{
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerRequest request)
    {
        try
        {
            var answer = await answerService.SubmitAnswerAsync(
                request.QuestionId,
                request.UserId,
                request.AnswerText,
                request.OptionsId
            );
            return Ok(answer);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAnswerById(Guid id)
    {
        try
        {
            var answer = await answerService.GetAnswerByIdAsync(id);
            return Ok(answer);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

    [HttpGet("survey/{surveyId}/options")]
    public async Task<IActionResult> GetOptionsWithAnswerCount(Guid surveyId)
    {
        try
        {
            var optionsWithCounts = await answerService.GetOptionsWithAnswerCountAsync(surveyId);
            return Ok(optionsWithCounts);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { message = "An error occurred while processing your request", details = ex.Message });
        }
    }
}