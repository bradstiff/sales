using System;
using System.Collections.Generic;
using System.Linq;

using Proposing.API.Domain.Core;
using Proposing.API.Domain.Exceptions;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public class ProductModelVersion : Enumeration<int>
    {
        public static ProductModelVersion Submitted = new ProductModelVersion(1, nameof(Submitted).ToLowerInvariant());

        protected ProductModelVersion()
        {
        }

        public ProductModelVersion(int id, string name)
            : base(id, name)
        {
        }
    }
}
