﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Mappings
{
    public class ProcedureMapping : IEntityTypeConfiguration<Procedure>
    {
        public void Configure(EntityTypeBuilder<Procedure> builder)
        {
            builder.ToTable("Procedures");

            builder.HasKey(p => p.ProcedureId);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(70);

            builder.Property(p => p.Value)
                .IsRequired()
                .HasPrecision(4, 2);

            builder.Property(p => p.InsertedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .IsRequired(false);

            /*
            builder.HasMany(p => p.Employees)
                .WithMany(e => e.Procedures)
                .UsingEntity<EmployeeProcedure>();
            */
        }
    }
}
