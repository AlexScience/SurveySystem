using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.DTO.DTO;

[SwaggerSchema("Model for updating multiple options.")]
public record OptionUpdateDto
{
    [Required] 
    [SwaggerSchema("The list of options to update.")]
    public List<OptionTextUpdate> Options { get; set; } = new();
}