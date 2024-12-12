namespace SurveySystem.Models.Models;

public record Option(Guid Id, string Text, Guid QuestionId)
{
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}