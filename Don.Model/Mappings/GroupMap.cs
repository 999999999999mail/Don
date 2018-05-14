using Don.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Mappings
{
    public class GroupMap : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Group");

            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Remark)
                .HasMaxLength(100);

            builder.Property(e => e.State)
                .HasDefaultValue(0);

            builder.Property(e => e.Sys)
                .HasDefaultValue(false);
        }
    }
}
