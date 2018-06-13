using Don.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Don.Model.Mappings
{
    public class RolePermitMap : IEntityTypeConfiguration<RolePermit>
    {
        public void Configure(EntityTypeBuilder<RolePermit> builder)
        {
            builder.ToTable("RolePermit");
        }
    }
}
