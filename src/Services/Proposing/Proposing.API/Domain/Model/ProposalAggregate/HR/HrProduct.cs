﻿using Proposing.API.Domain.Core;
using Proposing.API.Domain.Model.ProposalAggregate.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public class HrProduct : Entity, IProduct, IProductScopeUpdater<HrProductScopeDto>
    {
        public short? LevelId { get; private set; }

        private readonly List<HrProductCountry> _productCountries;
        public IReadOnlyCollection<HrProductCountry> ProductCountries => _productCountries;

        public ProductType ProductType => ProductType.HR;

        public HrProduct()
        {
            _productCountries = new List<HrProductCountry>();
        }

        public void Update(HrProductScopeDto scope)
        {
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