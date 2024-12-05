namespace SurveySystem.Models.Models;

public record Counter(Guid Id, Guid SurveyId)
{
    public List<string> UserId { get; set; } = new();
}