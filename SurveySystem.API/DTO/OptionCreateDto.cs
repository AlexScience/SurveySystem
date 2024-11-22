using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.API.DTO;

public class OptionCreateDto
{
    [Required]
    [MaxLength(250)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public Guid QuestionId { get; set; }
}