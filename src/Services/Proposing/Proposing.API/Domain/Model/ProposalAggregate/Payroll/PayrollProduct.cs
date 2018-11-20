using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate.Payroll
{
    public class PayrollProduct : Entity, IProduct, IProductScopeUpdater<PayrollProductScopeDto>
    {
        public bool? Reporting { get; private set; }
        public bool? PayslipStorage { get; private set; }

        private readonly List<PayrollProductCountry> _productCountries;
        public IReadOnlyCollection<PayrollProductCountry> ProductCountries => _productCountries;

        public PayrollProduct()
        {
            _productCountries = new List<PayrollProductCountry>();
        }

        public void Update(PayrollProductScopeDto scope)
        {
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
