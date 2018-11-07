using System;
using System.Collections.Generic;
using System.Linq;

using Proposing.Domain.Exceptions;

namespace Proposing.Domain.Core
{
    public class ProductType : Enumeration<long>
    {
        public static ProductType Payroll = new ProductType(1, nameof(Payroll).ToLowerInvariant());
        public static ProductType HR = new ProductType(1 << 1, nameof(HR).ToLowerInvariant());
        public static ProductType Time = new ProductType(1 << 2, nameof(Time).ToLowerInvariant());
        public static ProductType Benefits = new ProductType(1 << 3, nameof(Benefits).ToLowerInvariant());

        public ProductType()
        {
        }

        public ProductType(long value, string name) : base(value, name)
        {
        }

        public static ProductType From(long value)
        {
            return ProductType.FromValue<ProductType>(value);
        }
    }
}
