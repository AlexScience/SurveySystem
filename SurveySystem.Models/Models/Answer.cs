namespace SurveySystem.Models.Models;

public record Answer(Guid Id, Guid QuestionId, string AnswerText, Guid? OptionId, string UserId)
{
    public Question Question { get; set; } = default!;
    public Option Option { get; set; } = default!;
    public User User { get; set; } = default!;
}