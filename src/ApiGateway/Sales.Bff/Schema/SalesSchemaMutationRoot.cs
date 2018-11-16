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
        }
    }
}
