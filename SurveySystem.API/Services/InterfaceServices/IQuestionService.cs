using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IQuestionService
{
    public Task<Question> UpdateQuestionAsync(Guid id, QuestionUpdateDto questionUpdateDto);
    public Task<Question> UpdateOptionAsync(Guid id, OptionUpdateDto optionUpdateDto);
    public Task<bool> DeleteQuestionAsync(Guid id);
}