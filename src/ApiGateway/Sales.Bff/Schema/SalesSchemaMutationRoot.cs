using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Proposing.SchemaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Schema
{
    public class SalesSchemaMutationRoot : ObjectGraphType
    {
        public SalesSchemaMutationRoot(ProposingClient client)
        {
            Field<ProposalSchemaType>(
                "CreateProposal",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CreateProposalSchemaType>> { Name = "proposal" }),
                resolve: context => client.Proposals_CreateProposalAsync(context.GetArgument<CreateProposalCommand>("proposal"))
            );

            FieldAsync<ProposalSchemaType>(
                "UpdateProposalCountries",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "proposalId" },
                    new QueryArgument<NonNullGraphType<ListGraphType<NonNullGraphType<ProposalCountryInputSchemaType>>>> { Name = "proposalCountries" }),
                resolve: async context =>
                {
                    var command = new UpdateProposalCountriesCommand
                    {
                        Countries = context.GetArgument<List<ProposalCountryDto>>("proposalCountries")
                    };
                    var proposalId = context.GetArgument<int>("proposalId");
                    await client.Proposals_UpdateCountriesAsync(proposalId, command);
                    return await client.Proposals_GetProposalAsync(proposalId);
                }
            );

            FieldAsync<ProposalSchemaType>(
                "UpdateHrScope",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "proposalId" },
                    new QueryArgument<NonNullGraphType<ListGraphType<NonNullGraphType<IntGraphType>>>> { Name = "countryIds" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "levelId" }
                    ),
                resolve: async context =>
                {
                    var command = new UpdateHrProductScopeCommand
                    {
                        LevelId = context.GetArgument<short>("levelId"),
                        CountryIds = context.GetArgument<List<int>>("countryIds"),
                    };
                    var proposalId = context.GetArgument<int>("proposalId");
                    await client.HrScope_UpdateScopeAsync(proposalId, command);
                    return await client.Proposals_GetProposalAsync(proposalId);
                }
            );
        }
    }
}
