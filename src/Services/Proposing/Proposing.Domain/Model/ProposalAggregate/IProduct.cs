using Proposing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate
{
    public interface IProduct
    {
        ProductType ProductType { get; }
        void Update(ProductScopeDto scope);
        void AddCountry();
        void DeleteCountry();
        void ChangeModelVersion();
        void GetStuffForPriceModel();
    }
}
