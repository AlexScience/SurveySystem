using Microsoft.AspNetCore.Mvc;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController(IQuestionService questionService) : ControllerBase
{
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateQuestion(Guid id, [FromBody] QuestionUpdateDto question)
    {
        var resultQuestion = await questionService.UpdateQuestionAsync(id, question);

        return Ok(resultQuestion);
    }
}