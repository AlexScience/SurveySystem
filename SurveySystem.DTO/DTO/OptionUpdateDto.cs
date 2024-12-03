using System.ComponentModel.DataAnnotations;

namespace SurveySystem.DTO.DTO;

public class OptionUpdateDto
{
    [Required] 
    [MaxLength(250)] 
    public string Text { get; set; }
}