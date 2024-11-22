using System.Text.Json.Serialization;

namespace SurveySystem.API.Models;


public record Question(Guid Id, string? Text, QuestionType Type, Guid SurveyId)
{
    [JsonIgnore]
    public Survey Survey { get; set; }
    
    public ICollection<Option> Options { get; set; } = new List<Option>();
}
