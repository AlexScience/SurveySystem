namespace SurveySystem.DTO.DTO;

public record QuestionWithOptionsDto
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; }

    public List<OptionWithAnswerCountDto> Options { get; set; }
}