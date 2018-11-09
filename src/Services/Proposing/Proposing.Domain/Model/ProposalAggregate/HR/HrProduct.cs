using Proposing.Domain.Core;
using Proposing.Domain.Model.ProposalAggregate.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate
{
    public class HrProduct : Entity, IProduct
    {
        public short? LevelId { get; private set; }

        private readonly List<HrProductCountry> _productCountries;
        public IReadOnlyCollection<HrProductCountry> ProductCountries => _productCountries;

        public ProductType ProductType => ProductType.HR;

        public HrProduct()
        {
            _productCountries = new List<HrProductCountry>();
        }

        public HrProduct(ProductScopeDto scope): this()
        {
            this.SetScopeValues(scope);
        }

        public void Update(ProductScopeDto scope)
        {
            this.SetScopeValues(scope);
        }

        private void SetScopeValues(ProductScopeDto productScope)
        {
            var scope = (HrProductScopeDto)productScope;
            LevelId = scope.LevelId;
            this.ProductCountries
                .Where(pc => !scope.CountryScopes.Any(countryScope => pc.CountryId == countryScope.CountryId))
                .ToList()
                .ForEach(country =>
                {
                    _productCountries.Remove(country);
                });
            foreach (var countryScope in scope.CountryScopes)
            {
                var productCountry = this.ProductCountries.FirstOrDefault(pc => pc.CountryId == countryScope.CountryId);
                if (productCountry == null)
                {
                    productCountry = new HrProductCountry(countryScope.CountryId);
                    _productCountries.Add(productCountry);
                }
                productCountry.SetScopeValues(scope.LevelId);
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
