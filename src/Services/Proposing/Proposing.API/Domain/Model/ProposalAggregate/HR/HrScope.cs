using Proposing.API.Domain.Core;
using Proposing.API.Domain.Model.ProposalAggregate.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public class HrScope : Entity, IProductScope, IProductScopeUpdater<HrScopeDto>
    {
        public short? LevelId { get; private set; }

        private readonly List<HrCountryScope> _countryScopes;
        public IReadOnlyCollection<HrCountryScope> CountryScopes => _countryScopes;

        public ProductType ProductType => ProductType.HR;

        public HrScope()
        {
            _countryScopes = new List<HrCountryScope>();
        }

        public void Update(HrScopeDto scope)
        {
            LevelId = scope.LevelId;
            this.CountryScopes
                .Where(pc => !scope.CountryScopes.Any(countryScope => pc.CountryId == countryScope.CountryId))
                .ToList()
                .ForEach(country =>
                {
                    _countryScopes.Remove(country);
                });
            foreach (var countryScope in scope.CountryScopes)
            {
                var productCountryScope = this.CountryScopes.FirstOrDefault(pc => pc.CountryId == countryScope.CountryId);
                if (productCountryScope == null)
                {
                    productCountryScope = new HrCountryScope(countryScope.CountryId);
                    _countryScopes.Add(productCountryScope);
                }
                productCountryScope.SetScopeValues(scope.LevelId);
            }
        }

        public void AddCountry()
        {
            throw new NotImplementedException();
        }

        public void DeleteCountry()
        {
            throw new NotImplementedException();
        }

        public void ChangeModelVersion()
        {
            throw new NotImplementedException();
        }

        public void GetStuffForPriceModel()
        {
            throw new NotImplementedException();
        }
    }
}
