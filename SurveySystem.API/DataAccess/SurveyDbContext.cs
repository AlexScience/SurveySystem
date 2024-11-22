using Microsoft.EntityFrameworkCore;
using SurveySystem.API.DataAccess.Configuration;
using SurveySystem.API.Models;

namespace SurveySystem.API.DataAccess;

public class SurveyDbContext(DbContextOptions<SurveyDbContext> options) : DbContext(options)
{
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Answer> Answers { get; set; }
    
    public DbSet<ApplicationUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AnswerConfiguration());
        modelBuilder.ApplyConfiguration(new OptionConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionConfiguration());
        modelBuilder.ApplyConfiguration(new SurveyConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}