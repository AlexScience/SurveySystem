using SurveySystem.API.DTO;

namespace SurveySystem.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.API.Models;

[ApiController]
[Route("api/[controller]")]
public class SurveysController(ISurveyService surveyService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateDto surveyDto)
    {
        if (surveyDto == null)
            return BadRequest("Invalid survey data");

        var createdSurvey = await surveyService.CreateSurveyAsync(surveyDto);
        return CreatedAtAction(nameof(GetSurveyById), new { id = createdSurvey.Id }, createdSurvey);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSurveyById(Guid id)
    {
        var survey = await surveyService.GetSurveyByIdAsync(id);
        if (survey == null)
            return NotFound();

        return Ok(survey);
    }
}