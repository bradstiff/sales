using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.DataLoader;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public partial class ProposalCountrySchemaType
    {
        partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
        {
            Field<HrProductCountrySchemaType, HrProductCountryViewModel>()
                .Name("hr")
                .ResolveAsync(context => {
                    var loader = accessor.Context.GetOrAddBatchLoader<Tuple<int, int>, HrProductCountryViewModel>(
                        "GetHrProductCountries",
                        async keys =>
                        {
                            var countriesByProposal = await Task.WhenAll(keys
                                .GroupBy(key => key.Item1)
                                .Select(group => client.HrProductScope_GetCountryScopeAsync(group.Key, null)));
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
