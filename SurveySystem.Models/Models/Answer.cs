using System.Text.Json.Serialization;

namespace SurveySystem.Models.Models;

public record Answer(Guid Id, Guid QuestionId, string? AnswerText, string UserId)
{
    [JsonIgnore]
    public Question Question { get; set; } = default!;
    [JsonIgnore]
    public User User { get; set; } = default!;
    public ICollection<Option> SelectedOptions { get; set; } = new List<Option>();
    [JsonIgnore]
    public Option? SelectedOption => SelectedOptions.Count == 1 ? SelectedOptions.First() : null;
}