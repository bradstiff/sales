using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public class ProposalCountry : Entity
    {
        public ProposalCountry(int countryId, int? headcount)
        {
            CountryId = countryId;
            Headcount = headcount;
        }

        private ProposalCountry()
        {
        }

        public int CountryId { get; private set; }
        public long ProductTypeIds { get; private set; }
        public int? Headcount { get; private set; }

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

        internal void SetHeadcount(int? headcount)
        {
            this.Headcount = headcount;
        }
    }
}
