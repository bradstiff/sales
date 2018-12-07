using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proposing.API.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Infrastructure.Context
{
    public class ComponentEntityConfiguration : IEntityTypeConfiguration<Component>
    {
        public void Configure(EntityTypeBuilder<Component> componentConfiguration)
        {
            componentConfiguration.ToTable("Component");
            componentConfiguration.HasKey(ct => ct.Id);
            componentConfiguration.Property(ct => ct.Id)
                .ValueGeneratedNever()
                .IsRequired();
            componentConfiguration.Property<short>("ComponentTypeId");
        }
    }
}
