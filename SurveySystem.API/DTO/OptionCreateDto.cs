using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.DTO;

public class OptionCreateDto
{
    [Required]
    [MaxLength(100)]
    [SwaggerSchema("The text of the option")]
    public string Text { get; set; } = string.Empty;
}