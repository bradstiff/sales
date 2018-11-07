using Proposing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate
{
    public class ProposalCountry : Entity
    {
        public ProposalCountry(int countryId)
        {
            CountryId = countryId;
            PayrollProductScope = new PayrollProductCountryScope();
            HrProductScope = new HrProductCountryScope();
        }

        private ProposalCountry()
        {
        }

        public int CountryId { get; private set; }
        public long ProductTypeIds { get; private set; }
        public PayrollProductCountryScope PayrollProductScope { get; private set; }
        public HrProductCountryScope HrProductScope { get; private set; }

        public bool HasProductType(ProductType productType)
        {
            return ProductType.IsFlagSet(this.ProductTypeIds, productType);
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

        public void SetHrProductScope(HrProductCountryScope scope)
        {
            if (this.HrProductScope != scope)
            {
                this.HrProductScope = scope;
            }
        }
    }
}
