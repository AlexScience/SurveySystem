using SurveySystem.DTO.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services.InterfaceServices;

[ApiController]
[Route("api/[controller]")]
public class SurveysController(ISurveyService surveyService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new survey",
        Description = "Creates a new survey with the specified title, description, and questions."
    )]
    public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateDto surveyDto)
    {
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

    [HttpGet("{surveyId}")]
    [SwaggerOperation(
        Summary = "Get a survey by ID",
        Description = "Retrieves the details of a survey by its unique identifier."
    )]
    public async Task<IActionResult> GetSurveyById(Guid surveyId)
    {
        if (surveyId == Guid.Empty)
        {
            return BadRequest("Survey ID cannot be empty.");
        }

        var survey = await surveyService.GetSurveyByIdAsync(surveyId);

        return Ok(survey);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Update a survey by ID",
        Description = "Editing the survey title and description"
    )]
    public async Task<IActionResult> UpdateSurvey(Guid id, [FromBody] SurveyUpdateDto surveyUpdateDto)
    {
        var resultSurvey = await surveyService.UpdateSurveyAsync(id, surveyUpdateDto);

        return Ok(resultSurvey);
    }
}