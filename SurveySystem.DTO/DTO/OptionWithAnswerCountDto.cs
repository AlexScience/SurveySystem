using System.Text.Json.Serialization;

namespace SurveySystem.DTO.DTO;

public class OptionWithAnswerCountDto
{
    public Guid OptionId { get; set; }
    public string Text { get; set; }
    public int AnswerCount { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<CreateUserRequestDto> AnsweredUsers { get; set; } = new(); // Список пользователей
}