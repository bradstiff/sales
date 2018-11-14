using GraphQL.Bff.Proposing.SchemaTypes;
using GraphQL.Types;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Bff.Proposing
{
    public class ProposingSchemaQueryRoot : ObjectGraphType
    {
        public ProposingSchemaQueryRoot(ProposalsClient client)
        {
            Field<ProposalType>(
                "Proposal",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => client.GetProposalAsync(context.GetArgument<int>("id"))
            );

            Field<ListGraphType<ProposalType>>(
                "Proposals",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "hasCountry" },
                    new QueryArgument<IntGraphType> { Name = "hasProduct" },
                    new QueryArgument<IntGraphType> { Name = "hasAnyProduct" }
                    ),
                resolve: context => client.GetProposalsAsync()
            );
        }
    }
}
