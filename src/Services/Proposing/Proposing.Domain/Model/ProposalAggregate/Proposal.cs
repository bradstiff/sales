using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using Proposing.Domain.Core;
using Proposing.Domain.Model.ProposalAggregate.Payroll;

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
        public PayrollProduct PayrollProduct { get; private set; }
        public HrProduct HrProduct { get; private set; }
        private readonly List<ProposalCountry> _proposalCountries;
        public IReadOnlyCollection<ProposalCountry> ProposalCountries => _proposalCountries;

        private List<IProduct> _products;
        public IReadOnlyCollection<IProduct> Products => _products = _products ?? this.TheProducts.ToList();

        IEnumerable<IProduct> TheProducts
        {
            get
            {
                if (this.PayrollProduct != null)
                    yield return this.PayrollProduct;
                if (this.HrProduct != null)
                    yield return this.HrProduct;
            }
        }
        private Proposal()
        {
            _proposalCountries = new List<ProposalCountry>();
            _products = new List<IProduct>();
        }

        public Proposal(IEnumerable<int> countryIds) : this()
        {
            ProductModelVersionId = 1; //todo
            PriceModelVersionId = 1;

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

        public void SetGeneralProperties(string name, string clientName, string comments)
        {
            Name = name;
            ClientName = clientName;
            Comments = comments;
        }

        public bool HasProduct(ProductType productType)
        {
            return ProductType.IsFlagSet(this.ProductTypeIds, productType);
        }

        public void SetProductScope<T>(ProductType productType, T scopeData) where T: ProductScopeDto
        {
            //validate that a ProposalCountry exists for every ProductCountry and that there are no duplicates
            var productCountries = (from countryScope in scopeData.CountryScopes
                             join proposalCountry in this.ProposalCountries on countryScope.CountryId equals proposalCountry.CountryId into joinedProposalCountries
                             from proposalCountry in joinedProposalCountries.DefaultIfEmpty()
                             select new
                             {
                                 countryScope.CountryId,
                                 ProposalCountry = proposalCountry ?? throw new ArgumentOutOfRangeException($"Country {countryScope.CountryId} is not in scope for Proposal {this.Id}")
                             }).ToDictionary(c => c.CountryId);
                            
            if (this.HasProduct(productType))
            {
                var product = this.Products.FirstOrDefault(p => p.ProductType == productType);
                product.Update(scopeData);
            }
            else
            {
                var product = productType.NewProductScopeInstance(scopeData);
                _products.Add(product);
                if (productType == ProductType.Payroll)
                    this.PayrollProduct = (PayrollProduct)product;
                else if (productType == ProductType.HR)
                    this.HrProduct = (HrProduct)product;
                this.ProductTypeIds |= productType.Value;
            }

            //adjust the ProposalCountry.ProductTypeIds
            this.ProposalCountries.ForEach(proposalCountry =>
            {
                var countryHasProduct = productCountries.Keys.Contains(proposalCountry.CountryId);
                var countryHadProduct = proposalCountry.HasProductType(productType);
                if (countryHasProduct && !countryHadProduct)
                {
                    proposalCountry.AddProductType(productType);
                }
                else if (countryHadProduct && !countryHasProduct)
                {
                    proposalCountry.RemoveProductType(productType);
                }
            });
        }

        public void RemoveProduct(ProductType productType) 
        {
            var product = this.Products.FirstOrDefault(p => p.ProductType == productType);
            _products.Remove(product);
            if (productType == ProductType.Payroll)
                this.PayrollProduct = null;
            else if (productType == ProductType.HR)
                this.HrProduct = null;
            this.ProductTypeIds &= ~productType.Value;
        }
    }
}
