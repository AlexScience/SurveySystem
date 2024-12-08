namespace SurveySystem.DTO.DTO;

public class AnswerCreateDto
{
    public Guid QuestionId { get; set; }
    public Guid? OptionId { get; set; } // Для одиночного выбора

    public List<Guid> OptionIds { get; set; } // Для множественного выбора
    public string AnswerText { get; set; } // Для текстового ответа
    public string UserId { get; set; }
}