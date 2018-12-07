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
    public class ProductCountryEntityConfiguration<T, TParent> : IEntityTypeConfiguration<T>
        where T : Entity, IProductCountry
        where TParent : Entity, IProduct
    {
        private string _tableName;
        public ProductCountryEntityConfiguration(string tableName)
        {
            _tableName = tableName;
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(_tableName);
            builder.HasKey(p => p.Id);
            builder.Property<int>("ProposalId").IsRequired();
            builder.HasAlternateKey(new[] { nameof(IProductCountry.CountryId), "ProposalId" });
            builder.HasOne<TParent>().WithMany("ProductCountries").HasForeignKey("ProposalId").HasPrincipalKey("ProposalId").OnDelete(DeleteBehavior.Cascade);
            builder.Ignore(p => p.DomainEvents);
        }
    }
}
