using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SurveySystem.DTO.DTO;

public class OptionCreateDto
{
    [Required]
    [MaxLength(250)]
    public string Text { get; set; } = string.Empty;
}