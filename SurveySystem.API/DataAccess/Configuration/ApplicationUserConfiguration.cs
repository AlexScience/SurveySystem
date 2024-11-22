using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveySystem.API.Models;

namespace SurveySystem.API.DataAccess.Configuration;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("users");

        builder.Property(u => u.FullName)
            .HasColumnType("varchar(150)")
            .HasMaxLength(200);
    }
}