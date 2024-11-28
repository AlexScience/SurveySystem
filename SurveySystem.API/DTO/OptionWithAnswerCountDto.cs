namespace SurveySystem.API.DTO;

public class OptionWithAnswerCountDto
{
    public Guid OptionId { get; set; }
    public string Text { get; set; }
    public int AnswerCount { get; set; }
}