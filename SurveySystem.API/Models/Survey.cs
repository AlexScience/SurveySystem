namespace SurveySystem.API.Models;

public record Survey(Guid Id, string? Title, string? Description, DateTime CreatedAt)
{
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}