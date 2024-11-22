using SurveySystem.API.Models;

namespace SurveySystem.API.DTO;

public class QuestionCreateDto
{
    public string Text { get; set; }
    public QuestionType Type { get; set; }
    public List<OptionCreateDto> Options { get; set; }
}