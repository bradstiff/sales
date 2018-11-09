using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proposing.Domain.Core;
using Proposing.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Proposing.API.Infrastructure.Context
{
    public class ProductConfiguration<T> : IEntityTypeConfiguration<T>
        where T:Entity, IProduct
    {
        private string _tableName;
        private Expression<Func<Proposal, T>> _property;
        public ProductConfiguration(string tableName, Expression<Func<Proposal, T>> property)
        {
            _tableName = tableName;
            _property = property;
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(_tableName);
            builder.HasKey(p => p.Id);
            //builder.Property<int>("ProposalId").IsRequired();
            builder.HasOne<Proposal>().WithOne(_property).HasForeignKey<T>("Id");
            builder.Ignore(p => p.DomainEvents);

            builder.Metadata
                .FindNavigation("ProductCountries")
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
