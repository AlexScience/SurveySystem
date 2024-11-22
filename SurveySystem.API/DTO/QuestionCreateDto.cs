using System.ComponentModel.DataAnnotations;
using SurveySystem.API.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.DTO;

public class QuestionCreateDto
{
    [Required]
    [MaxLength(200)]
    [SwaggerSchema("The text of the question")]
    public string Text { get; set; } = string.Empty;

    [Required]
    [SwaggerSchema("The type of the question (MultipleChoice or OpenEnded)")]
    public QuestionType Type { get; set; }

    [SwaggerSchema("The possible options for the question, if applicable")]
    public ICollection<OptionCreateDto>? Options { get; set; }
}
