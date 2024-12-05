using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;

namespace SurveySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController(IQuestionService questionService) : ControllerBase
{
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] QuestionUpdateDto question)
    {
        var resultQuestion = await questionService.UpdateQuestionAsync(id, question);

        return Ok(resultQuestion);
    }

    [HttpPut("update-option/{id:guid}")]
    public async Task<IActionResult> UpdateOptions(Guid id, [FromBody] OptionUpdateDto optionUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedQuestion = await questionService.UpdateOptionAsync(id, optionUpdateDto);
            return Ok(updatedQuestion);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { message = "An error occurred while updating the options.", details = ex.Message });
        }
    }
}