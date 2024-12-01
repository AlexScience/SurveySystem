using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.DTO.DTO;

public record QuestionUpdateDto()
{
    [Required]
    [MaxLength(500)]
    [SwaggerSchema("The text of the question")]
    public string Text { get; set; } = string.Empty;
    
}