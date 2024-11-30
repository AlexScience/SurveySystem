using SurveySystem.DTO.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;

[ApiController]
[Route("api/[controller]")]
public class SurveysController (ISurveyService surveyService, IAnswerService answerService) : ControllerBase
{
    // Создание нового опроса
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new survey",
        Description = "Creates a new survey with the specified title, description, and questions."
    )]
    public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateDto surveyDto)
    {
        if (surveyDto == null)
        {
            return BadRequest("Survey data is required.");
        }

        try
        {
            var createdSurvey = await surveyService.CreateSurveyAsync(surveyDto);
            return CreatedAtAction(nameof(GetSurveyById), new { id = createdSurvey.Id }, createdSurvey);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // Получение опроса по ID
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a survey by ID",
        Description = "Retrieves the details of a survey by its unique identifier."
    )]
    public async Task<IActionResult> GetSurveyById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Survey ID cannot be empty.");
        }

        var survey = await surveyService.GetSurveyByIdAsync(id);
        if (survey == null)
        {
            return NotFound($"Survey with ID {id} not found.");
        }

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
