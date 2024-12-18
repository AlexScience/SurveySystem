﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveySystem.Models.Models;

namespace SurveySystem.API.DataAccess.Configuration;

public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
{
    public void Configure(EntityTypeBuilder<Survey> builder)
    {
        builder.ToTable("surveys");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Title)
            .HasMaxLength(500);
        builder.Property(s => s.Description)
            .HasMaxLength(1000);
        builder.Property(s => s.CreatedAt)
            .IsRequired();
        builder.Property(s => s.Type)
            .IsRequired();
    }
}