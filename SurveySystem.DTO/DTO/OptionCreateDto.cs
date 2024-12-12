using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.DTO.DTO;

[SwaggerSchema("Model for creating an option.")]
public class OptionCreateDto
{
    [Required]
    [MaxLength(500)]
    [SwaggerSchema("The text of the option (maximum 250 characters).")]
    public string Text { get; set; } = string.Empty;
}