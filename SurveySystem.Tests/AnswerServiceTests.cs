using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.Tests;

public class AnswerServiceTests
{
    private readonly SurveyDbContext _dbContext;
    private readonly AnswerService _answerService;

    public AnswerServiceTests()
    {
        // Используем InMemoryDatabase для теста
        var options = new DbContextOptionsBuilder<SurveyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new SurveyDbContext(options);
        _answerService = new AnswerService(_dbContext);
    }

    [Fact]
    public async Task CreateAnswerAsync_ShouldReturnAnswer_WhenValidDataIsProvided()
    {
        // Arrange
        var questionId = Guid.NewGuid();
        var userId = "user123";
        var optionId = Guid.NewGuid();
        var answerDto = new AnswerCreateDto
        {
            QuestionId = questionId,
            AnswerText = "My Answer",
            OptionId = optionId,
            UserId = userId
        };

        // Mock data
        var question = new Question(questionId, "Sample Question", QuestionType.MultipleChoice, Guid.NewGuid())
        {
            Options = new List<Option>
            {
                new Option(Guid.NewGuid(), "Option 1", questionId),
                new Option(optionId, "Option 2", questionId)
            }
        };

        var user = new ApplicationUser { Id = userId, UserName = "user123" };

        // Сохраняем данные в InMemory базу
        _dbContext.Questions.Add(question);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // Act
        var answer = await _answerService.CreateAnswerAsync(answerDto);

        // Assert
        Assert.NotNull(answer);
        Assert.Equal(answerDto.QuestionId, answer.QuestionId);
        Assert.Equal(answerDto.AnswerText, answer.AnswerText);
        Assert.Equal(answerDto.OptionId, answer.OptionId);
        Assert.Equal(answerDto.UserId, answer.UserId);
    }
}
