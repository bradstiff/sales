using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proposing.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Infrastructure.Context
{
    public class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
    {
        public void Configure(EntityTypeBuilder<Proposal> builder)
        {
            builder.ToTable("Proposal");
            builder.Ignore(p => p.DomainEvents);
            builder.HasKey(p => p.Id);

            builder.OwnsOne(o => o.PayrollProductScope);
            builder.OwnsOne(o => o.HrProductScope);

            builder.Metadata
                .FindNavigation(nameof(Proposal.ProposalCountries))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
