﻿using Don.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Mappings
{
    public class AdminMap : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admin");

            builder.Property(e => e.LoginName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Password)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.RealName)
                .HasMaxLength(50);

            builder.Property(e => e.Tel)
                .HasMaxLength(50);

            builder.Property(e => e.EMail)
                .HasMaxLength(50);

            builder.Property(e => e.CreateTime)
                .IsRequired();

            builder.Property(e => e.State)
                .HasDefaultValue(0);
        }
    }
}
