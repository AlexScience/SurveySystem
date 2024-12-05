using System.Text.Json.Serialization;

namespace SurveySystem.Models.Models;

public record Answer(Guid Id, Guid QuestionId, string AnswerText, Guid? OptionId, string UserId)
{
    [JsonIgnore] public Question Question { get; set; } = default!;
    [JsonIgnore] public Option Option { get; set; } = default!;
    [JsonIgnore] public User User { get; set; } = default!;
}