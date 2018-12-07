using GraphQL.Types;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{

    public partial class ProposalSchemaType : ObjectGraphType<ProposalViewModel>
    {
        partial void Extend(ProposingClient client)
        {
            Field<HrProductSchemaType>(
                "hr",
                resolve: null//context => queries.GetHrProduct(context.Source.Id)
                );
        }
    }
}
