using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IQuestionService
{
    Task<Question> UpdateQuestionAsync(Guid id, QuestionUpdateDto questionUpdateDto);
    Task<bool> DeleteQuestionAsync(Guid id);
}