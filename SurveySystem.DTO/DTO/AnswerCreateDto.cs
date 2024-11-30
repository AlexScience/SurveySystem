namespace SurveySystem.DTO.DTO;

public class AnswerCreateDto
{
    public Guid QuestionId { get; set; }
    public string AnswerText { get; set; }
    public Guid? OptionId { get; set; }
    public string UserId { get; set; } // Используйте идентификатор пользователя для связи с Identity
}