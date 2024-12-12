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
        builder.Property(a => a.AnswerText).HasMaxLength(1000);
        builder.HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(a => a.SelectedOptions)
            .WithMany(o => o.Answers)
            .UsingEntity<Dictionary<string, object>>(
                "AnswerSelectedOptions",
                join => join
                    .HasOne<Option>()
                    .WithMany()
                    .HasForeignKey("OptionId")
                    .OnDelete(DeleteBehavior.Cascade),
                join => join
                    .HasOne<Answer>()
                    .WithMany()
                    .HasForeignKey("AnswerId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}