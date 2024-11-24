using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.DTO;
using SurveySystem.API.Models;
using SurveySystem.API.Services.InterfaceServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswerController(IAnswerService answerService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Submit an answer to a question",
        Description = "Allows a user to submit an answer to a specific question."
    )]
    public async Task<IActionResult> SubmitAnswer([FromBody] AnswerCreateDto answerDto)
    {
        if (answerDto == null)
            return BadRequest("Invalid answer data");

        var answer = await answerService.CreateAnswerAsync(answerDto);
        return CreatedAtAction(nameof(GetAnswerById), new { id = answer.Id }, answer);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get an answer by ID",
        Description = "Retrieves the details of an answer using its unique identifier."
    )]
    public async Task<IActionResult> GetAnswerById(Guid id)
    {
        var answer = await answerService.GetAnswerByIdAsync(id);
        if (answer == null)
            return NotFound();

        return Ok(answer);
    }
    
    [HttpGet("options/{questionId}")]
    [SwaggerOperation(
        Summary = "Get options with answer count",
        Description = "Retrieves the details of an answer using its unique identifier."
    )]
    public async Task<IActionResult> GetOptionsWithAnswerCount(Guid questionId)
    {
        var optionsWithAnswerCount = await answerService.GetOptionsWithAnswerCountAsync(questionId);
        return Ok(optionsWithAnswerCount);
    }

}