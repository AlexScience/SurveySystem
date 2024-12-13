using SurveySystem.API.Services.InterfaceServices;
using SurveySystem.DTO.DTO;

namespace SurveySystem.UI.Services;

public class SurveyService (HttpClient httpClient) : ISurveyService
{
    public async Task<SurveyWithAnswerCountDto> CreateSurveyAsync(SurveyCreateDto surveyDto)
    {
        var response = await httpClient.PostAsJsonAsync("api/surveys/create", surveyDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SurveyWithAnswerCountDto>();
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Failed to create survey: {error}");
    }

    // Get a survey by ID
    public async Task<SurveyWithAnswerCountDto> GetSurveyByIdAsync(Guid surveyId)
    {
        var response = await httpClient.GetAsync($"api/surveys/view-survey/{surveyId:guid}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SurveyWithAnswerCountDto>();
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Failed to retrieve survey: {error}");
    }

    // Update an existing survey
    public async Task<SurveyWithAnswerCountDto> UpdateSurveyAsync(Guid surveyId, SurveyUpdateDto surveyUpdateDto)
    {
        var response = await httpClient.PutAsJsonAsync($"api/surveys/{surveyId}", surveyUpdateDto);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<SurveyWithAnswerCountDto>();
        }

        var error = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Failed to update survey: {error}");
    }
}
