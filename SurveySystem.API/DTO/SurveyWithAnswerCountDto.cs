namespace SurveySystem.API.DTO;

public class SurveyWithAnswerCountDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<QuestionWithOptionsDto> Questions { get; set; }
}