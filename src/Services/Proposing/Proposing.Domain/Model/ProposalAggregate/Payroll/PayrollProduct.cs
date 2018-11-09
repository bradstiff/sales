using Proposing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate.Payroll
{
    public class PayrollProduct : Entity, IProduct
    {
        public bool? Reporting { get; private set; }
        public bool? PayslipStorage { get; private set; }

        public ProductType ProductType => ProductType.Payroll;

        private readonly List<PayrollProductCountry> _productCountries;
        public IReadOnlyCollection<PayrollProductCountry> ProductCountries => _productCountries;

        public PayrollProduct()
        {
        }

        public PayrollProduct(ProductScopeDto scope)
        {
            this.SetScopeValues(scope);
        }

        public void Update(ProductScopeDto scope)
        {
            this.SetScopeValues(scope);
        }

        private void SetScopeValues(ProductScopeDto productScope)
        {
            var scope = (PayrollProductScopeDto)productScope;
            Reporting = scope.Reporting;
            PayslipStorage = scope.PayslipStorage;
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
