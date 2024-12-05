using System.ComponentModel.DataAnnotations;

namespace SurveySystem.DTO.DTO;

public class OptionUpdateDto
{
    [Required] 
    public List<OptionTextUpdate> Options { get; set; } = new();
}