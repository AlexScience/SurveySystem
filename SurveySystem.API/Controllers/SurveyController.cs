using SurveySystem.API.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.API.Models;

[ApiController]
[Route("api/[controller]")]
public class SurveysController(ISurveyService surveyService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new survey",
        Description = "Creates a new survey with the specified title, description, and questions."
    )]
    [SwaggerResponse(201, "Survey created successfully", typeof(SurveyCreateDto))]
    [SwaggerResponse(400, "Invalid survey data")]
    [SwaggerResponse(500, "Internal server error")]
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
    [SwaggerResponse(200, "Survey retrieved successfully", typeof(SurveyCreateDto))]
    [SwaggerResponse(404, "Survey not found")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetSurveyById(Guid id)
    {
        var survey = await surveyService.GetSurveyByIdAsync(id);
        if (survey == null)
            return NotFound();

        return Ok(survey);
    }
}