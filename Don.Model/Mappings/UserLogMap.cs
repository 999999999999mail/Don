using Don.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Mappings
{
    public class UserLogMap : IEntityTypeConfiguration<UserLog>
    {
        public void Configure(EntityTypeBuilder<UserLog> builder)
        {
            builder.ToTable("UserLog");

            builder.Property(e => e.Desc)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Keywords)
                .HasMaxLength(100);

            builder.Property(e => e.IP)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Website)
                .HasMaxLength(200);
        }
    }
}
