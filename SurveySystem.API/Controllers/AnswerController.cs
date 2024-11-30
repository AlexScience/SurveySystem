using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswerController(IAnswerService answerService) : ControllerBase
{
    // Метод для отправки ответа
    [HttpPost("create")]
    [SwaggerOperation(
        Summary = "Submit an answer to a question",
        Description = "Allows a user to submit an answer to a specific question."
    )]
    public async Task<IActionResult> SubmitAnswer([FromBody] AnswerCreateDto answerDto)
    {
        if (answerDto == null)
            return BadRequest("Invalid answer data");

        var answer = await answerService.CreateAnswerAsync(answerDto);
        return CreatedAtAction(nameof(SubmitAnswer), new { id = answer.Id }, answer);
    }

    // Метод для получения опций с количеством ответов
    [HttpGet("options/{questionId}")]
    [SwaggerOperation(
        Summary = "Get options with answer count",
        Description = "Retrieves the options for a specific question along with the count of answers submitted for each option."
    )]
    public async Task<IActionResult> GetOptionsWithAnswerCount(Guid questionId)
    {
        var optionsWithAnswerCount = await answerService.GetOptionsWithAnswerCountAsync(questionId);
        return Ok(optionsWithAnswerCount);
    }
}