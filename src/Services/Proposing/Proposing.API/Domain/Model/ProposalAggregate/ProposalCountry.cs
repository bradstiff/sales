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
        public long ProductIds { get; private set; }
        public int? Headcount { get; private set; }

        public bool HasProductType(ProductType productType)
        {
            return ProductType.IsFlagSet(this.ProductIds, productType);
        }

        public void AddProductType(ProductType productType)
        {
            if (!this.HasProductType(productType))
            {
                this.ProductIds |= productType.Value;
            }
        }

        public void RemoveProductType(ProductType productType)
        {
            if (this.HasProductType(productType))
            {
                this.ProductIds &= ~productType.Value;
            }
        }

        internal void SetHeadcount(int? headcount)
        {
            this.Headcount = headcount;
        }
    }
}
