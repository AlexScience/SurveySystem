using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace SurveySystem.DTO.DTO;

[SwaggerSchema("Request model for creating a user.")]
public record CreateUserRequestDto
{
    
    [JsonIgnore]
    [SwaggerSchema("The unique identifier of the user (ignored during creation).")]
    public string Id { get; init; } = string.Empty;

    [SwaggerSchema("The username of the user.")]
    public string Username { get; set; } = string.Empty;

    [SwaggerSchema("The password for the user account.")]
    public string Password { get; set; } = string.Empty;

    [SwaggerSchema("The full name of the user.")]
    public string FullName { get; set; } = string.Empty;

    [SwaggerSchema("The email address of the user.")]
    public string Email { get; set; } = string.Empty;
}