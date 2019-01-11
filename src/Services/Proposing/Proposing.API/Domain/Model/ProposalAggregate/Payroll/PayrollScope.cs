using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate.Payroll
{
    public class PayrollScope : Entity, IProductScope, IProductScopeUpdater<PayrollScopeDto, PayrollCountryScopeDto>
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
                    productCountryScope = new PayrollCountryScope(countryScope.CountryId);
                    _countryScopes.Add(productCountryScope);
                }
                productCountryScope.SetScopeValues(countryScope.LevelId, countryScope.WeeklyPayees, countryScope.BiWeeklyPayees, countryScope.SemiMonthlyPayees, countryScope.MonthlyPayees, scope.Reporting, scope.PayslipStorage);
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
