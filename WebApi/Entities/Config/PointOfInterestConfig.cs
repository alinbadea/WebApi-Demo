using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities.Config
{
    public class PointOfInterestConfig : IEntityTypeConfiguration<PointOfInterest>
    {
        public void Configure(EntityTypeBuilder<PointOfInterest> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        }
    }
}
