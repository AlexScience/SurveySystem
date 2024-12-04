using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveySystem.Models.Models;

namespace SurveySystem.API.DataAccess.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(u => u.FullName)
            .HasColumnType("varchar(150)")
            .HasMaxLength(200);
    }
}