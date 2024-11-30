using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveySystem.Models.Models;

namespace SurveySystem.API.DataAccess.Configuration;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.ToTable("answers");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AnswerText)
            .HasMaxLength(500);

        builder.HasOne(a => a.Question)
            .WithMany()
            .HasForeignKey(a => a.QuestionId);

        builder.HasOne(a => a.Option)
            .WithMany()
            .HasForeignKey(a => a.OptionId);

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId);
    }
}