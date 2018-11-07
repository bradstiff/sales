using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using Proposing.Domain.Core;

namespace Proposing.Domain.Model.ProposalAggregate
{
    public class Proposal : Entity
    {
        public string Name { get; private set; }
        public string ClientName { get; private set; }
        public string Comments { get; private set; }
        public int ProductModelVersionId { get; private set; }
        public int PriceModelVersionId { get; private set; }
        public long ProductTypeIds { get; private set; }

        public PayrollProductGlobalScope PayrollProductScope { get; private set; }
        public HrProductGlobalScope HrProductScope { get; private set; }

        private readonly List<ProposalCountry> _proposalCountries;
        public IReadOnlyCollection<ProposalCountry> ProposalCountries => _proposalCountries;

        private Proposal()
        {
            _proposalCountries = new List<ProposalCountry>();
        }

        public Proposal(IEnumerable<int> countryIds) : this()
        {
            ProductModelVersionId = 1; //todo
            PriceModelVersionId = 1;

            //EF Core 2.1 does not allow owned types to be null, so we have to initialize them.
            //Logic uses InScopeProductTypeIds rather than ProductScope properties to determine if a product is in scope.
            PayrollProductScope = new PayrollProductGlobalScope();
            HrProductScope = new HrProductGlobalScope();

            if (countryIds != null)
            {
                foreach (var countryId in countryIds)
                {
                    _proposalCountries.Add(new ProposalCountry(countryId));
                }
            }
        }

        public void UpdateCountries(IEnumerable<int> countryIds)
        {
            var changes = false;
            this.ProposalCountries
                .Where(pc => !countryIds.Any(id => pc.CountryId == id))
                .ToList()
                .ForEach(country =>
                {
                    _proposalCountries.Remove(country);
                    changes = true;
                });
            foreach(var countryId in countryIds.Where(id => !this.ProposalCountries.Any(pc => pc.CountryId == id)))
            {
                _proposalCountries.Add(new ProposalCountry(countryId));
                changes = true;
            }
            if (changes)
            {
                //todo
            }
        }

        public bool HasProductType(ProductType productType)
        {
            return ProductType.IsFlagSet(this.ProductTypeIds, productType);
        }

        public void SetGeneralProperties(string name, string clientName, string comments)
        {
            Name = name;
            ClientName = clientName;
            Comments = comments;
        }

        public void AddProductType(ProductType productType)
        {
            if (!this.HasProductType(productType))
            {
                this.ProductTypeIds |= productType.Value;
            }
        }

        public void RemoveProductType(ProductType productType)
        {
            if (this.HasProductType(productType))
            {
                this.ProductTypeIds &= ~productType.Value;
            }
        }

        //todo: might be better to move to services (open-closed principle)
        public void SetPayrollProductScope(PayrollProductGlobalScope scope)
        {
            if (this.PayrollProductScope != scope)
            {
                this.PayrollProductScope = scope;
            }
        }

        public void SetHrProductScope(HrProductGlobalScope scope)
        {
            if (this.HrProductScope != scope)
            {
                this.HrProductScope = scope;
            }
        }
    }
}
