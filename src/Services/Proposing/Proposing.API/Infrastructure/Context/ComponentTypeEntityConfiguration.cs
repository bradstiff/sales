using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proposing.API.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Infrastructure.Context
{
    public class ComponentTypeEntityConfiguration: IEntityTypeConfiguration<ComponentType>
    {
        public void Configure(EntityTypeBuilder<ComponentType> builder)
        {
            builder.ToTable("ComponentType");
            builder.HasKey(ct => ct.Id);
            builder.Property(ct => ct.Id)
                .ValueGeneratedNever()
                .IsRequired();
        }
    }
}
