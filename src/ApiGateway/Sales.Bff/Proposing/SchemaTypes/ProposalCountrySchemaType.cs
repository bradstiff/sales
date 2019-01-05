using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public partial class ProposalCountrySchemaType
    {
        partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
        {
            Field<ListGraphType<ProductDefinitionSchemaType>>(
                "products",
                resolve: context =>
                {
                    var model = cache.ProductModels[context.Source.ProductModelId];
                    return context.Source.ProductIds
                        .GetBits()
                        .Select(id => model.Products.First(p => p.Id == id));
                }
            );
            Field<PayrollCountryScopeSchemaType, PayrollCountryScopeViewModel>()
                .Name("payrollScope")
                .ResolveAsync(context => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Tuple<int, int>, PayrollCountryScopeViewModel>(
                        "GetPayrollCountryScopes",
                        async keys =>
                        {
                            var countriesByProposal = await Task.WhenAll(keys
                                .GroupBy(key => key.Item1)
                                .Select(group => client.PayrollScope_GetCountryScopeAsync(group.Key, null)));
                            return countriesByProposal
                                .SelectMany(batch => batch)
                                .ToDictionary(c => Tuple.Create(c.ProposalId, c.CountryId));
                        }
                    );
                    return loader.LoadAsync(Tuple.Create(context.Source.ProposalId, context.Source.CountryId));
                }
            );
            Field<HrCountryScopeSchemaType, HrCountryScopeViewModel>()
                .Name("hrScope")
                .ResolveAsync(context => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Tuple<int, int>, HrCountryScopeViewModel>(
                        "GetHrCountryScopes",
                        async keys =>
                        {
                            var countriesByProposal = await Task.WhenAll(keys
                                .GroupBy(key => key.Item1)
                                .Select(group => client.HrScope_GetCountryScopeAsync(group.Key, null)));
                            return countriesByProposal
                                .SelectMany(batch => batch)
                                .ToDictionary(c => Tuple.Create(c.ProposalId, c.CountryId));
                        }
                    );
                    return loader.LoadAsync(Tuple.Create(context.Source.ProposalId, context.Source.CountryId));
                }
            );
        }
    }
}
