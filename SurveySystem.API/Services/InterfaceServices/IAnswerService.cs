using SurveySystem.API.DTO;
using SurveySystem.API.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IAnswerService
{
    public Task<Answer> GetAnswerByIdAsync(Guid id);
    public Task<Answer> CreateAnswerAsync(AnswerCreateDto answerDto);
    public Task<List<OptionWithAnswerCountDto>> GetOptionsWithAnswerCountAsync(Guid questionId);
}