using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.DTO.DTO;

[SwaggerSchema("Model for updating the text of an option.")]
public record OptionTextUpdate()
{
    [Required]
    [SwaggerSchema("The unique identifier of the option to update.")]
    public Guid OptionId { get; set; }

    [Required]
    [MaxLength(500)]
    [SwaggerSchema("The updated text of the option (maximum 250 characters).")]
    public string Text { get; set; } = string.Empty;
}