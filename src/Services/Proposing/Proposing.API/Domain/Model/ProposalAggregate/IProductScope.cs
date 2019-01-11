using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public interface IProductScope
    {
        void AddCountry();
        void DeleteCountry();
        void ChangeModelVersion();
        void GetStuffForPriceModel();
    }

    public interface IProductScopeUpdater<TScopeDto, TCountryScopeDto> : IProductScope 
        where TScopeDto : ProductScopeDto<TCountryScopeDto>
        where TCountryScopeDto: ProductCountryScopeDto
    {
        void Update(TScopeDto scopeDto);
    }
}
