using SurveySystem.API.DTO;

namespace SurveySystem.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.API.Models;

[ApiController]
[Route("api/[controller]")]
public class OptionsController(IOptionService optionService) : ControllerBase
{
    
    [HttpGet("question/{questionId:guid}")]
    public async Task<IActionResult> GetOptionsByQuestionId(Guid questionId)
    {
        var options = await optionService.GetOptionsByQuestionIdAsync(questionId);
        return Ok(options);
    }

    [HttpGet("byId{id:guid}")]
    public async Task<IActionResult> GetOptionById(Guid id)
    {
        var option = await optionService.GetOptionByIdAsync(id);
        if (option == null) return NotFound();
        return Ok(option);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOption(OptionCreateDto optionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var option = new Option(Guid.NewGuid(), optionDto.Text,optionDto.QuestionId);
        var createdOption = await optionService.CreateOptionAsync(option);
        return CreatedAtAction(nameof(GetOptionById), new { id = createdOption.Id }, createdOption);
    }

    [HttpPut("update{id:guid}")]
    public async Task<IActionResult> UpdateOption(Guid id, Option option)
    {
        if (id != option.Id) return BadRequest();

        var updatedOption = await optionService.UpdateOptionAsync(option);
        return Ok(updatedOption);
    }

    [HttpDelete("delete{id:guid}")]
    public async Task<IActionResult> DeleteOption(Guid id)
    {
        var isDeleted = await optionService.DeleteOptionAsync(id);
        if (!isDeleted) return NotFound();

        return NoContent();
    }
}
