using System;
using System.Collections.Generic;
using System.Linq;

using Proposing.Domain.Exceptions;
using Proposing.Domain.Model.ProposalAggregate;
using Proposing.Domain.Model.ProposalAggregate.HR;
using Proposing.Domain.Model.ProposalAggregate.Payroll;

namespace Proposing.Domain.Core
{
    public class ProductType : Enumeration<long>
    {
        public static ProductType Payroll = new ProductType(1, nameof(Payroll).ToLowerInvariant(), scopeData => new PayrollProduct((PayrollProductScopeDto)scopeData));
        public static ProductType HR = new ProductType(1 << 1, nameof(HR).ToLowerInvariant(), scopeData => new HrProduct((HrProductScopeDto)scopeData));
        public static ProductType Time = new ProductType(1 << 2, nameof(Time).ToLowerInvariant(), null);
        public static ProductType Benefits = new ProductType(1 << 3, nameof(Benefits).ToLowerInvariant(), null);

        public Func<ProductScopeDto, IProduct> NewProductScopeInstance { get; private set; }

        public ProductType()
        {
        }

        public ProductType(long value, string name, Func<ProductScopeDto, IProduct> newProductScopeInstance) : base(value, name)
        {
            this.NewProductScopeInstance = newProductScopeInstance;
        }

        public static ProductType From(long value)
        {
            return ProductType.FromValue<ProductType>(value);
        }
    }
}
