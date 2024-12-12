using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IAnswerService
{
    public Task<Answer> SubmitAnswerAsync(Guid questionId, string userId, string? answerText, IEnumerable<Guid>? optionsId);
    public Task<Answer> GetAnswerByIdAsync(Guid id);
    public Task<List<OptionWithAnswerCountDto>> GetOptionsWithAnswerCountAsync(Guid questionId);
}