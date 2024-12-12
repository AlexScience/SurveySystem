using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.DTO.DTO;

[SwaggerSchema("Option model with the count of associated answers.")]
public record OptionWithAnswerCountDto
{
    [Required]
    [SwaggerSchema("The unique identifier of the option.")]
    public Guid OptionId { get; set; }

    [Required]
    [MaxLength(500)]
    [SwaggerSchema("The text of the option.")]
    public string Text { get; set; } = string.Empty;

    [SwaggerSchema("The number of answers associated with the option.")]
    public int AnswerCount { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [SwaggerSchema("The list of users who answered this option.", Nullable = true)]
    public List<CreateUserRequestDto> AnsweredUsers { get; set; } = new();
}