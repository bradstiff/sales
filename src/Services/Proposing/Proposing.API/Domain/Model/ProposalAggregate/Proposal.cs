using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreLinq;
using Proposing.API.Application.Commands;
using Proposing.API.Domain.Core;
using Proposing.API.Domain.Model.ProposalAggregate.Payroll;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public class Proposal : Entity
    {
        public string Name { get; private set; }
        public string ClientName { get; private set; }
        public string Comments { get; private set; }
        public int ProductModelId { get; private set; }
        public int PriceModelId { get; private set; }
        public long ProductIds { get; private set; }

        public PayrollScope PayrollScope { get; private set; }
        public HrScope HrScope { get; private set; }

        private readonly List<ProposalCountry> _proposalCountries;
        public IReadOnlyCollection<ProposalCountry> ProposalCountries => _proposalCountries;

        private Dictionary<ProductType, Func<IProductScope>> _productConstructors;
        private Dictionary<ProductType, Func<Proposal, IProductScope>> _productGetters;
        private Dictionary<ProductType, Action<Proposal, IProductScope>> _productSetters;

        private Proposal()
        {
            _proposalCountries = new List<ProposalCountry>();
        }

        public Proposal(IEnumerable<ProposalCountryDto> countries) : this()
        {
            ProductModelId = 1; //todo
            PriceModelId = 1;

            if (countries != null)
            {
                foreach (var country in countries)
                {
                    _proposalCountries.Add(new ProposalCountry(country.CountryId, country.Headcount));
                }
            }
        }

        #region Product Generalization Methods
        private IProductScope NewProduct(ProductType productType)
        {
            if (_productConstructors == null)
            {
                _productConstructors = ProductType
                    .GetAll<ProductType>()
                    .Select(pt => new
                    {
                        ProductType = pt,
                        Constructor = ExpressionUtils.CreateDefaultConstructor<IProductScope>(pt.Type)
                    })
                    .ToDictionary(x => x.ProductType, x => x.Constructor);
            }
            return _productConstructors[productType]();
        }

        private IProductScope GetProduct(ProductType productType)
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

        private void SetProduct(ProductType productType, IProductScope product)
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

        public void UpdateCountries(IEnumerable<ProposalCountryDto> countries)
        {
            var changes = false;
            this.ProposalCountries
                .Where(pc => !countries.Any(country => pc.CountryId == country.CountryId))
                .ToList()
                .ForEach(country =>
                {
                    _proposalCountries.Remove(country);
                    changes = true;
                });
            foreach(var country in countries)
            {
                var proposalCountry = this.ProposalCountries.FirstOrDefault(pc => pc.CountryId == country.CountryId);
                if (proposalCountry == null)
                {
                    _proposalCountries.Add(new ProposalCountry(country.CountryId, country.Headcount));
                    changes = true;
                }
                else if (proposalCountry.Headcount != country.Headcount)
                {
                    proposalCountry.SetHeadcount(country.Headcount);
                    changes = true;
                }
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
            //check entity or check flags? 
            //flags would surface cases where the product was not included in the query
            //entity is vulnerable to flags getting out of sync
            return this.GetProduct(productType) != null;
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
                var product = (IProductScopeUpdater<T>)this.GetProduct(productType);
                product.Update(scopeData);
            }
            else
            {
                var product = (IProductScopeUpdater<T>)this.NewProduct(productType);
                product.Update(scopeData);
                this.SetProduct(productType, product);
                this.ProductIds |= productType.Value;
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
            this.ProductIds &= ~productType.Value;
        }
    }
}
