using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.DTO;
using SurveySystem.API.Models;
using SurveySystem.API.Services.InterfaceServices;

namespace SurveySystem.API.Controllers;

[ApiController]
[Route("api/answers")]
public class AnswerController : ControllerBase
{
    private readonly IAnswerService _answerService;

    public AnswerController(IAnswerService answerService)
    {
        _answerService = answerService;
    }

    // Ответить на вопрос
    [HttpPost]
    public async Task<IActionResult> SubmitAnswer([FromBody] AnswerCreateDto answerDto)
    {
        if (answerDto == null)
            return BadRequest("Invalid answer data");

        var answer = await _answerService.CreateAnswerAsync(answerDto);
        return CreatedAtAction(nameof(GetAnswerById), new { id = answer.Id }, answer);
    }

    // Получение ответа по ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAnswerById(Guid id)
    {
        var answer = await _answerService.GetAnswerByIdAsync(id);
        if (answer == null)
            return NotFound();

        return Ok(answer);
    }
}


