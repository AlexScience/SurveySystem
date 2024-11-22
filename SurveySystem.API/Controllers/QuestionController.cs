namespace SurveySystem.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.API.Models;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController (IQuestionService questionService): ControllerBase
{

    [HttpGet("survey/{surveyId:guid}")]
    public async Task<IActionResult> GetQuestionsBySurveyId(Guid surveyId)
    {
        var questions = await questionService.GetQuestionsBySurveyIdAsync(surveyId);
        return Ok(questions);
    }

    [HttpGet("byId{id:guid}")]
    public async Task<IActionResult> GetQuestionById(Guid id)
    {
        var question = await questionService.GetQuestionByIdAsync(id);
        if (question == null) return NotFound();
        return Ok(question);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateQuestion(Question question)
    {
        var createdQuestion = await questionService.CreateQuestionAsync(question);
        return CreatedAtAction(nameof(GetQuestionById), new { id = createdQuestion.Id }, createdQuestion);
    }

    [HttpPut("update{id:guid}")]
    public async Task<IActionResult> UpdateQuestion(Guid id, Question question)
    {
        if (id != question.Id) return BadRequest();

        var updatedQuestion = await questionService.UpdateQuestionAsync(question);
        return Ok(updatedQuestion);
    }

    [HttpDelete("delete{id:guid}")]
    public async Task<IActionResult> DeleteQuestion(Guid id)
    {
        var isDeleted = await questionService.DeleteQuestionAsync(id);
        if (!isDeleted) return NotFound();

        return NoContent();
    }
}
