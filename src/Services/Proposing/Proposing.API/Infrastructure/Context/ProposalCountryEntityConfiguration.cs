using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proposing.API.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Infrastructure.Context
{
    public class ProposalCountryEntityConfiguration : IEntityTypeConfiguration<ProposalCountry>
    {
        public void Configure(EntityTypeBuilder<ProposalCountry> builder)
        {
            builder.ToTable("ProposalCountry");
            builder.HasKey(p => p.Id);
            builder.Property<int>("ProposalId").IsRequired();
            builder.HasAlternateKey(new[] { nameof(ProposalCountry.CountryId), "ProposalId" });
            builder.Ignore(p => p.DomainEvents);
        }
    }
}
