using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;
using Sales.Bff.Proposing.SchemaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Schema
{
    public class SalesSchemaQueryRoot : ObjectGraphType
    {
        public SalesSchemaQueryRoot(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
        {
            Field<ProposalSchemaType>(
                "Proposal",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => client.Proposals_GetProposalAsync(context.GetArgument<int>("id"))
            );

            Field<ProposalListPageSchemaType>(
                "ProposalListPage",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "page", DefaultValue = 1 },
                    new QueryArgument<IntGraphType> { Name = "rowsPerPage", DefaultValue = 25 },
                    new QueryArgument<StringGraphType> { Name = "orderBy", DefaultValue = "name" },
                    new QueryArgument<StringGraphType> { Name = "order", DefaultValue = "asc" },
                    new QueryArgument<IntGraphType> { Name = "hasProduct" },
                    new QueryArgument<IntGraphType> { Name = "hasAnyProduct" }
                    ),
                resolve: context => client.Proposals_GetProposalsAsync(
                    context.GetArgument<int>("page"), 
                    context.GetArgument<int>("rowsPerPage"))
            );

            Field<ListGraphType<CountrySchemaType>>(
                "Countries",
                resolve: context => client.Countries_GetCountriesAsync()
            );

            Field<ListGraphType<ProductModelSchemaType>>(
                "ProductModels",
                resolve: context => cache.ProductModels.Values
            );
        }
    }
}
