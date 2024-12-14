using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.Tests;

public class SurveyServiceTests
{
    private SurveyDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<SurveyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SurveyDbContext(options);
    }

    [Fact]
    public async Task CreateSurveyAsync_Should_Create_Survey_With_Valid_Data()
    {
        var dbContext = CreateInMemoryDbContext();
        var surveyService = new SurveyService(dbContext);

        var surveyDto = new SurveyCreateDto
        {
            Title = "Test Survey",
            Description = "A description",
            Type = SurveyType.PublicType,
            Questions = new List<QuestionCreateDto>
            {
                new QuestionCreateDto
                {
                    Text = "Question 1",
                    Type = QuestionType.MultipleChoice,
                    Options = new List<OptionCreateDto>
                    {
                        new OptionCreateDto { Text = "Option 1" },
                        new OptionCreateDto { Text = "Option 2" }
                    }
                }
            }
        };

        var result = await surveyService.CreateSurveyAsync(surveyDto);

        Assert.NotNull(result);
        Assert.Equal(surveyDto.Title, result.Title);
        Assert.Equal(1, result.Questions.Count);
    }

    [Fact]
    public async Task CreateSurveyAsync_Should_Throw_If_QuestionText_Is_Empty()
    {
        var dbContext = CreateInMemoryDbContext();
        var surveyService = new SurveyService(dbContext);

        var surveyDto = new SurveyCreateDto
        {
            Title = "Test Survey",
            Description = "A description",
            Type = SurveyType.PublicType,
            Questions = new List<QuestionCreateDto>
            {
                new QuestionCreateDto
                {
                    Text = "",
                    Type = QuestionType.MultipleChoice
                }
            }
        };

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await surveyService.CreateSurveyAsync(surveyDto));
    }

    [Fact]
    public async Task GetSurveyByIdAsync_Should_Return_Correct_Survey()
    {
        var dbContext = CreateInMemoryDbContext();
        var survey = new Survey(Guid.NewGuid(), "Test Survey", "Description", DateTime.UtcNow, SurveyType.PublicType);
        dbContext.Surveys.Add(survey);
        await dbContext.SaveChangesAsync();

        var surveyService = new SurveyService(dbContext);

        var result = await surveyService.GetSurveyByIdAsync(survey.Id);

        Assert.NotNull(result);
        Assert.Equal(survey.Id, result.Id);
        Assert.Equal(survey.Title, result.Title);
    }

    [Fact]
    public async Task GetSurveyByIdAsync_Should_Throw_If_Survey_Not_Found()
    {
        var dbContext = CreateInMemoryDbContext();
        var surveyService = new SurveyService(dbContext);
        var nonExistentId = Guid.NewGuid();

        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await surveyService.GetSurveyByIdAsync(nonExistentId));
    }
}