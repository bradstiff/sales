using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Proposing.API.Domain.Exceptions;
using Proposing.API.Domain.Model.ProposalAggregate;
using Proposing.API.Domain.Model.ProposalAggregate.HR;
using Proposing.API.Domain.Model.ProposalAggregate.Payroll;

namespace Proposing.API.Domain.Core
{
    public class ProductType : Enumeration<long>
    {
        public static ProductType Payroll = new ProductType(1, nameof(Payroll).ToLowerInvariant(), typeof(PayrollProduct), p => p.PayrollProduct);
        public static ProductType HR = new ProductType(1 << 1, nameof(HR).ToLowerInvariant(), typeof(HrProduct), p => p.HrProduct);
        //public static ProductType Time = new ProductType(1 << 2, nameof(Time).ToLowerInvariant(), null);
        //public static ProductType Benefits = new ProductType(1 << 3, nameof(Benefits).ToLowerInvariant(), null);

        public Type Type { get; private set; }
        public Expression<Func<Proposal,IProduct>> Selector { get; private set; }

        public ProductType()
        {
        }

        public ProductType(long value, string name, Type product, Expression<Func<Proposal,IProduct>> selector) : base(value, name)
        {
            this.Selector = selector;
            this.Type = product;
        }

        public static ProductType From(long value)
        {
            return FromValue<ProductType>(value);
        }
    }
}
