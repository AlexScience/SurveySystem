using System.ComponentModel.DataAnnotations;

namespace SurveySystem.Models.Models;

public class SubmitAnswerRequest
{
    [Required]
    public Guid QuestionId { get; set; }

    [Required]
    public string UserId { get; set; } = default!;

    public string? AnswerText { get; set; }

    public IEnumerable<Guid>? OptionsId { get; set; }
}