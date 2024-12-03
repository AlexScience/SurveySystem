using System.Text.Json.Serialization;

namespace SurveySystem.Models.Models;

public record Option(Guid Id, string Text, Guid QuestionId)
{
    [JsonIgnore]
    public Question Question { get; set; }
}