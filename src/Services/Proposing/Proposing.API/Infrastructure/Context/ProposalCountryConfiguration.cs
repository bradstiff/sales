using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proposing.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Infrastructure.Context
{
    public class ProposalCountryConfiguration : IEntityTypeConfiguration<ProposalCountry>
    {
        public void Configure(EntityTypeBuilder<ProposalCountry> builder)
        {
            builder.ToTable("ProposalCountry");
            builder.Ignore(p => p.DomainEvents);
            builder.HasKey(p => p.Id);
            builder.Property<int>("ProposalId").IsRequired();
            builder.OwnsOne(o => o.PayrollProductScope);
            builder.OwnsOne(p => p.HrProductScope);
        }
    }
}
