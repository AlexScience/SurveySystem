using SurveySystem.API.Models;

namespace SurveySystem.API.Services.InterfaceServices;

public interface IQuestionService
{
    Task<IEnumerable<Question>> GetQuestionsBySurveyIdAsync(Guid surveyId);
    Task<Question?> GetQuestionByIdAsync(Guid id);
    Task<Question> CreateQuestionAsync(Question question);
    Task<Question> UpdateQuestionAsync(Question question);
    Task<bool> DeleteQuestionAsync(Guid id);
}