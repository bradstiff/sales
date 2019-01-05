using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate.Payroll
{
    public class PayrollScope : Entity, IProductScope, IProductScopeUpdater<PayrollScopeDto>
    {
        public bool? Reporting { get; private set; }
        public bool? PayslipStorage { get; private set; }

        private readonly List<PayrollCountryScope> _countryScopes;
        public IReadOnlyCollection<PayrollCountryScope> CountryScopes => _countryScopes;

        public PayrollScope()
        {
            _countryScopes = new List<PayrollCountryScope>();
        }

        public void Update(PayrollScopeDto scope)
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
