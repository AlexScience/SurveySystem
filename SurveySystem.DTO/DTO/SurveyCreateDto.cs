﻿using System.ComponentModel.DataAnnotations;
using SurveySystem.Models.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.DTO.DTO;

public class SurveyCreateDto
{
    [Required]
    [MaxLength(100)]
    [SwaggerSchema("The title of the survey")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    [SwaggerSchema("A detailed description of the survey")]
    public string? Description { get; set; } = default!;
    
    [Required]
    [SwaggerSchema("Type of survey (Public, anonymous)")]
    public SurveyType Type { get; set; }

    [Required]
    [SwaggerSchema("A collection of questions associated with the survey")]
    public ICollection<QuestionCreateDto> Questions { get; set; } = new List<QuestionCreateDto>();
    
}