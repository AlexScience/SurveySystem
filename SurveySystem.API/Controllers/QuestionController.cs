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
    public async Task<IActionResult> UpdateOption(Guid id, [FromBody] OptionUpdateDto optionCreateDto)
    {
        var resultOption = await questionService.UpdateOptionAsync(id, optionCreateDto);

        return Ok(resultOption);
    }

    
}