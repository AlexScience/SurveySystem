namespace SurveySystem.Models.Models;

public record Survey(Guid Id, string? Title, string? Description, DateTime CreatedAt,SurveyType Type)
{
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    
}