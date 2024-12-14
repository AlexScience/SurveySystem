using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.API.Services;

public class AnswerService(SurveyDbContext context) : IAnswerService
{
    public async Task<Answer> SubmitAnswerAsync(Guid questionId, string userId, string? answerText,
        IEnumerable<Guid>? optionsId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

        if (questionId == Guid.Empty)
            throw new ArgumentException("Question ID cannot be empty.", nameof(questionId));

        var question = await context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == questionId);

        if (question == null)
            throw new ArgumentException("Invalid question ID.");

        switch (question.Type)
        {
            case QuestionType.SingleChoice:
                if (optionsId == null || optionsId.Count() != 1)
                    throw new ArgumentException("For single choice questions, exactly one option must be selected.");
                break;

            case QuestionType.MultipleChoice:
                if (optionsId == null || !optionsId.Any())
                    throw new ArgumentException("For multiple choice questions, at least one option must be selected.");
                break;

            case QuestionType.TextAnswer:
                if (string.IsNullOrWhiteSpace(answerText))
                    throw new ArgumentException("For text questions, an answer text must be provided.");
                break;

            default:
                throw new InvalidOperationException("Unsupported question type.");
        }

        if ((question.Type == QuestionType.SingleChoice || question.Type == QuestionType.MultipleChoice)
            && optionsId != null
            && optionsId.Any(id => !question.Options.Any(o => o.Id == id)))
        {
            throw new ArgumentException("One or more selected options do not belong to the specified question.");
        }

        var answer = new Answer(Guid.NewGuid(), questionId, question.Type == QuestionType.TextAnswer ? answerText : null, userId)
        {
            SelectedOptions = question.Type != QuestionType.TextAnswer
                ? question.Options.Where(o => optionsId!.Contains(o.Id)).ToList()
                : new List<Option>()
        };

        context.Answers.Add(answer);
        await context.SaveChangesAsync();

        return answer;
    }

    public async Task<Answer> GetAnswerByIdAsync(Guid id)
    {
        var answer = await context.Answers.AsNoTracking().Include(a => a.Question)
            .ThenInclude(q => q.Options)
            .Include(a => a.User)
            .Include(a => a.SelectedOptions)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (answer == null)
            throw new KeyNotFoundException($"Answer with ID {id} not found.");

        return answer;
    }

    public async Task<List<OptionWithAnswerCountDto>> GetOptionsWithAnswerCountAsync(Guid surveyId)
    {
        var survey = await context.Surveys
            .Include(s => s.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(s => s.Id == surveyId);

        if (survey == null)
            throw new KeyNotFoundException($"Survey with ID {surveyId} not found.");

        var questionsId = survey.Questions.Select(q => q.Id).ToList();

        var optionAnswerCounts = await context.Answers
            .Where(a => questionsId.Contains(a.QuestionId) && a.SelectedOptions.Any())
            .SelectMany(a => a.SelectedOptions)
            .GroupBy(option => option.Id)
            .Select(g => new
            {
                OptionId = g.Key,
                AnswerCount = g.Count()
            })
            .ToListAsync();

        var optionAnswerCountsDict = optionAnswerCounts.ToDictionary(o => o.OptionId, o => o.AnswerCount);

        var result = survey.Questions
            .SelectMany(q => q.Options)
            .Select(option => new OptionWithAnswerCountDto
            {
                OptionId = option.Id,
                Text = option.Text,
                AnswerCount =
                    optionAnswerCountsDict.GetValueOrDefault(option.Id, 0)
            })
            .ToList();

        return result;
    }
}