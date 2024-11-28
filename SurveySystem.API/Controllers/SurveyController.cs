using SurveySystem.API.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.API.Models;

[ApiController]
[Route("api/[controller]")]
public class SurveysController(ISurveyService surveyService, IAnswerService answerService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new survey",
        Description = "Creates a new survey with the specified title, description, and questions."
    )]
    public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateDto surveyDto)
    {
        var createdSurvey = await surveyService.CreateSurveyAsync(surveyDto);
        return CreatedAtAction(nameof(GetSurveyById), new { id = createdSurvey.Id }, createdSurvey);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a survey by ID",
        Description = "Retrieves the details of a survey by its unique identifier."
    )]
    public async Task<IActionResult> GetSurveyById(Guid id)
    {
        // Получаем информацию об опросе
        var survey = await surveyService.GetSurveyByIdAsync(id);
        if (survey == null)
            return NotFound();

        // Для каждого вопроса в опросе, получаем варианты и их количество ответов
        var surveyWithAnswerCount = new SurveyWithAnswerCountDto
        {
            Id = survey.Id,
            Title = survey.Title,
            Description = survey.Description,
            CreatedAt = survey.CreatedAt,
            Questions = new List<QuestionWithOptionsDto>()
        };

        foreach (var question in survey.Questions)
        {
            var optionsWithAnswerCount = await answerService.GetOptionsWithAnswerCountAsync(question.Id);
            surveyWithAnswerCount.Questions.Add(new QuestionWithOptionsDto
            {
                QuestionId = question.Id,
                QuestionText = question.Text,
                Options = optionsWithAnswerCount
            });
        }

        return Ok(surveyWithAnswerCount);
    }
}