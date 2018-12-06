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
            Field<ProposalType>(
                "CreateProposal",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CreateProposalType>> { Name = "proposal" }),
                resolve: context => client.Proposals_CreateProposalAsync(context.GetArgument<CreateProposalCommand>("proposal"))
            );
            FieldAsync<ProposalType>(
                "UpdateProposalCountries",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "proposalId" },
                    new QueryArgument<NonNullGraphType<ListGraphType<NonNullGraphType<ProposalCountryInputType>>>> { Name = "proposalCountries" }),
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
        }
    }
}
