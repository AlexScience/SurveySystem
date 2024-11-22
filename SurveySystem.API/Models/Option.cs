namespace SurveySystem.API.Models;

public record Option(Guid Id, string Text, Guid QuestionId)
{
    public Question Question { get; set; }
}
