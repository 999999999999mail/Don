using Don.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Mappings
{
    public class AdminRoleMap : IEntityTypeConfiguration<AdminRole>
    {
        public void Configure(EntityTypeBuilder<AdminRole> builder)
        {
            builder.ToTable("AdminRole");
        }
    }
}
