using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.DTO;
using SurveySystem.API.Models;
using SurveySystem.API.Services.InterfaceServices;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.Controllers;

[ApiController]
[Route("api/answers")]
public class AnswerController(IAnswerService answerService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Submit an answer to a question",
        Description = "Allows a user to submit an answer to a specific question."
    )]
    [SwaggerResponse(201, "Answer submitted successfully", typeof(AnswerCreateDto))]
    [SwaggerResponse(400, "Invalid answer data")]
    [SwaggerResponse(500, "Internal server error")]
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
    [SwaggerResponse(200, "Answer retrieved successfully", typeof(AnswerCreateDto))]
    [SwaggerResponse(404, "Answer not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetAnswerById(Guid id)
    {
        var answer = await answerService.GetAnswerByIdAsync(id);
        if (answer == null)
            return NotFound();

        return Ok(answer);
    }

}