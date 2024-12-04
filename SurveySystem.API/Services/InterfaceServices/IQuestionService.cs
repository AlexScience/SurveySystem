using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IQuestionService
{
    Task<Question> UpdateQuestionAsync(Guid id, QuestionUpdateDto questionUpdateDto);
    Task<Question> UpdateOptionAsync(Guid id, OptionUpdateDto optionCreateDto);
    Task<bool> DeleteQuestionAsync(Guid id);
}