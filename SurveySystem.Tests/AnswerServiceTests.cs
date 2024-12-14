using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess;
using SurveySystem.API.Services;
using SurveySystem.Models.Models;

namespace SurveySystem.Tests;

public class AnswerServiceTests
{
    private SurveyDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<SurveyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SurveyDbContext(options);
    }

    [Fact]
    public async Task SubmitAnswerAsync_Should_Create_Answer_For_TextQuestion()
    {
        var dbContext = CreateInMemoryDbContext();
        var question = new Question(Guid.NewGuid(), "Text question", QuestionType.TextAnswer, Guid.NewGuid());
        dbContext.Questions.Add(question);
        await dbContext.SaveChangesAsync();

        var answerService = new AnswerService(dbContext);
        var answerText = "Test Answer";
        
        var result = await answerService.SubmitAnswerAsync(question.Id, "user1", answerText, null);
        
        Assert.NotNull(result);
        Assert.Equal(answerText, result.AnswerText);
        Assert.Equal(question.Id, result.QuestionId);
    }

    [Fact]
    public async Task SubmitAnswerAsync_Should_Throw_If_Question_Not_Found()
    {
        var dbContext = CreateInMemoryDbContext();
        var answerService = new AnswerService(dbContext);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await answerService.SubmitAnswerAsync(Guid.NewGuid(), "user1", "Test", null));
    }

   
}