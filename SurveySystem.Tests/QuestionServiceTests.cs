using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services;
using SurveySystem.DTO.DTO;
using SurveySystem.Models.Models;

namespace SurveySystem.Tests;

public class QuestionServiceTests
{
    private SurveyDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<SurveyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SurveyDbContext(options);
    }

    [Fact]
    public async Task UpdateQuestionAsync_Should_Update_Question_Text()
    {
        var dbContext = CreateInMemoryDbContext();
        var question = new Question(Guid.NewGuid(), "Original Text", QuestionType.MultipleChoice, Guid.NewGuid());
        dbContext.Questions.Add(question);
        await dbContext.SaveChangesAsync();

        var questionService = new QuestionService(dbContext);
        var updateDto = new QuestionUpdateDto { Text = "Updated Text" };

        var result = await questionService.UpdateQuestionAsync(question.Id, updateDto);

        Assert.NotNull(result);
        Assert.Equal("Updated Text", result.Text);
    }

    [Fact]
    public async Task UpdateQuestionAsync_Should_Throw_If_Question_Not_Found()
    {
        var dbContext = CreateInMemoryDbContext();
        var questionService = new QuestionService(dbContext);
        var updateDto = new QuestionUpdateDto { Text = "Updated Text" };

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await questionService.UpdateQuestionAsync(Guid.NewGuid(), updateDto));
    }

    [Fact]
    public async Task DeleteQuestionAsync_Should_Remove_Question()
    {
        var dbContext = CreateInMemoryDbContext();
        var question = new Question(Guid.NewGuid(), "Text", QuestionType.TextAnswer, Guid.NewGuid());
        dbContext.Questions.Add(question);
        await dbContext.SaveChangesAsync();

        var questionService = new QuestionService(dbContext);

        var result = await questionService.DeleteQuestionAsync(question.Id);

        Assert.True(result);
        Assert.Empty(dbContext.Questions);
    }

    [Fact]
    public async Task DeleteQuestionAsync_Should_Return_False_If_Question_Not_Found()
    {
        var dbContext = CreateInMemoryDbContext();
        var questionService = new QuestionService(dbContext);

        var result = await questionService.DeleteQuestionAsync(Guid.NewGuid());

        Assert.False(result);
    }
}