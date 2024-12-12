using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SurveySystem.Models.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.DTO.DTO;

[SwaggerSchema("Model for a survey with the count of answers.")]
public class SurveyWithAnswerCountDto
{
    [Required]
    [SwaggerSchema("The unique identifier of the survey.")]
    public Guid Id { get; set; }


    [MaxLength(500)]
    [SwaggerSchema("The title of the survey.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    [SwaggerSchema("The description of the survey.")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [SwaggerSchema("The type of the survey.")]
    public SurveyType Type { get; set; }

    [Required]
    [SwaggerSchema("The creation date and time of the survey.")]
    public DateTime CreatedAt { get; set; }

    [SwaggerSchema("The list of questions in the survey.")]
    public List<QuestionWithOptionsDto> Questions { get; set; } = new();
}