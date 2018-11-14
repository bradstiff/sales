using GraphQL.Types;
using Proposing.API.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Bff.Proposing.SchemaTypes
{

    public class ProposalType : ObjectGraphType<Proposal>
    {
        public ProposalType(ProposingApiClient client)
        {
            Name = "Proposal";
            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.Name);
            Field(x => x.ClientName);
            Field<HrProductType>(
                "hr",
                resolve: null//context => queries.GetHrProduct(context.Source.Id)
                );
        }
    }
}
