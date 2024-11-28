using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.DTO;

public class OptionCreateDto
{
    [Required]
    [MaxLength(250)]
    public string Text { get; set; } = string.Empty;

    [Required]
    [JsonIgnore]
    public Guid QuestionId { get; set; }
}