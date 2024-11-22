namespace SurveySystem.API.DTO;

public class SurveyCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<QuestionCreateDto> Questions { get; set; }
}