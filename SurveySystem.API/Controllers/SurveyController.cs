using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services.InterfaceServices;

[ApiController]
[Route("api/[controller]")]
public class SurveysController(ISurveyService surveyService) : ControllerBase
{
    [HttpPost("create")]
    [SwaggerOperation(
        Summary = "Create a new survey",
        Description = "Creates a new survey with the specified title, description, questions, and survey type."
    )]
    public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateDto surveyDto)
    {
        if (!ModelState.IsValid) 
        {
            return BadRequest(ModelState); 
        }

        try
        {
            if (!Enum.IsDefined(typeof(SurveyType), surveyDto.Type))
            {
                return BadRequest("Invalid survey type.");
            }

            var createdSurvey = await surveyService.CreateSurveyAsync(surveyDto);
            return CreatedAtAction(nameof(GetSurveyById), new { surveyId = createdSurvey.Id }, createdSurvey);
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

    [HttpGet("/view-survey/{surveyId:guid}")]
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

        try
        {
            var survey = await surveyService.GetSurveyByIdAsync(surveyId);

            return Ok(survey);
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { message = "An error occurred while processing your request", details = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Update a survey by ID",
        Description = "Editing the survey title, description, and type"
    )]
    public async Task<IActionResult> UpdateSurvey(Guid id, [FromBody] SurveyUpdateDto surveyUpdateDto)
    {
        try
        {
            if (!Enum.IsDefined(typeof(SurveyType), surveyUpdateDto.Type))
            {
                return BadRequest("Invalid survey type.");
            }

            var updatedSurvey = await surveyService.UpdateSurveyAsync(id, surveyUpdateDto);

            return Ok(updatedSurvey);
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
}