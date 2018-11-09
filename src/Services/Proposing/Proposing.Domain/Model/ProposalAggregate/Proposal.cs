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

        private Dictionary<ProductType, Func<IProduct>> _productConstructors;
        private Dictionary<ProductType, Func<Proposal, IProduct>> _productGetters;
        private Dictionary<ProductType, Action<Proposal, IProduct>> _productSetters;

        private Proposal()
        {
            _proposalCountries = new List<ProposalCountry>();
            this.PayrollProduct = new PayrollProduct();
            this.HrProduct = new HrProduct();
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

        #region Product Generalization Methods
        private IProduct NewProduct(ProductType productType)
        {
            if (_productConstructors == null)
            {
                _productConstructors = ProductType
                    .GetAll<ProductType>()
                    .Select(pt => new
                    {
                        ProductType = pt,
                        Constructor = ExpressionUtils.CreateDefaultConstructor<IProduct>(pt.Type)
                    })
                    .ToDictionary(x => x.ProductType, x => x.Constructor);
            }
            return _productConstructors[productType]();
        }

        private IProduct GetProduct(ProductType productType)
        {
            if (_productGetters == null)
            {
                _productGetters = ProductType
                    .GetAll<ProductType>()
                    .Select(pt => new
                    {
                        ProductType = pt,
                        Getter = pt.Selector.ToGetter()
                    })
                    .ToDictionary(x => x.ProductType, x => x.Getter);
            }
            return _productGetters[productType](this);
        }

        private void SetProduct(ProductType productType, IProduct product)
        {
            if (_productSetters == null)
            {
                _productSetters = ProductType
                    .GetAll<ProductType>()
                    .Select(pt => new
                    {
                        ProductType = pt,
                        Setter = pt.Selector.ToSetter()
                    })
                    .ToDictionary(x => x.ProductType, x => x.Setter);
            }
            _productSetters[productType](this, product);
        }
        #endregion

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
                var product = this.GetProduct(productType);
                product.Update(scopeData);
            }
            else
            {
                var product = this.NewProduct(productType);
                product.Update(scopeData);
                this.SetProduct(productType, product);
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

        private void RemoveProduct(ProductType productType) 
        {
            this.SetProduct(productType, null);
            this.ProductTypeIds &= ~productType.Value;
        }
    }
}
