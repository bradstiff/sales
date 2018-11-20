using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public interface IProduct
    {
        void AddCountry();
        void DeleteCountry();
        void ChangeModelVersion();
        void GetStuffForPriceModel();
    }

    public interface IProductScopeUpdater<T> : IProduct where T:ProductScopeDto
    {
        void Update(T scope);
    }
}
