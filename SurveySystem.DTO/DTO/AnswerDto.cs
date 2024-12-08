namespace SurveySystem.DTO.DTO;

public record AnswerDto()
{
    public Guid QuestionId { get; set; }
    public object Answer { get; set; } // Ответ может быть строкой или списком вариантов
}