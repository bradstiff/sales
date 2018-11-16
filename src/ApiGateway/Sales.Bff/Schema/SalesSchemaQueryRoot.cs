using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Proposing.SchemaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Schema
{
    public class SalesSchemaQueryRoot : ObjectGraphType
    {
        public SalesSchemaQueryRoot(ProposingClient client)
        {
            Field<ProposalType>(
                "Proposal",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => client.Proposals_GetProposalAsync(context.GetArgument<int>("id"))
            );

            Field<ListGraphType<ProposalType>>(
                "Proposals",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "hasCountry" },
                    new QueryArgument<IntGraphType> { Name = "hasProduct" },
                    new QueryArgument<IntGraphType> { Name = "hasAnyProduct" }
                    ),
                resolve: context => client.Proposals_GetProposalsAsync()
            );
        }
    }
}
