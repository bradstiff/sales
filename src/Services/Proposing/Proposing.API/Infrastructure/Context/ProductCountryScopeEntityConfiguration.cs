using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proposing.API.Domain.Core;
using Proposing.API.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Infrastructure.Context
{
    public class ProductCountryScopeEntityConfiguration<T, TParent> : IEntityTypeConfiguration<T>
        where T : Entity, IProductCountryScope
        where TParent : Entity, IProductScope
    {
        private string _tableName;
        public ProductCountryScopeEntityConfiguration(string tableName)
        {
            _tableName = tableName;
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(_tableName);
            builder.HasKey(p => p.Id);
            builder.Property<int>("ProposalId").IsRequired();
            builder.HasAlternateKey(new[] { nameof(IProductCountryScope.CountryId), "ProposalId" });
            builder.HasOne<TParent>().WithMany("CountryScopes").HasForeignKey("ProposalId").HasPrincipalKey("ProposalId").OnDelete(DeleteBehavior.Cascade);
            builder.Ignore(p => p.DomainEvents);
        }
    }
}
