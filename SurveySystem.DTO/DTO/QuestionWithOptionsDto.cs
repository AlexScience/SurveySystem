using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.DTO.DTO;

[SwaggerSchema("Model for a question with its options.")]
public record QuestionWithOptionsDto
{
    [Required]
    [SwaggerSchema("The unique identifier of the question.")]
    public Guid QuestionId { get; set; }

    [MaxLength(1000)]
    [SwaggerSchema("The text of the question.")]
    public string QuestionText { get; set; } = default!;

    [SwaggerSchema("The list of options for the question.")]
    public List<OptionWithAnswerCountDto> Options { get; set; } = new();
}