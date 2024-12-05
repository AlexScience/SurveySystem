using System.ComponentModel.DataAnnotations;

namespace SurveySystem.DTO.DTO;

public record OptionTextUpdate()
{
    
    [Required]
    public Guid OptionId { get; set; }

    [Required]
    [MaxLength(250)]
    public string Text { get; set; } = string.Empty;
}