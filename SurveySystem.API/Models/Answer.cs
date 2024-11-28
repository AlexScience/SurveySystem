namespace SurveySystem.API.Models;

public record Answer(Guid Id, Guid QuestionId, string AnswerText, Guid? OptionId, string UserId)
{
    public Question Question { get; set; } = default!;
    public Option Option { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;
}