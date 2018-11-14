using GraphQL.Types;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{

    public class ProposalType : ObjectGraphType<Proposal>
    {
        public ProposalType(ProposalsClient client)
        {
            Name = "Proposal";
            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.Name);
            Field(x => x.ClientName);
            Field(x => x.Countries, type: typeof(ListGraphType<ProposalCountryType>));
            Field<HrProductType>(
                "hr",
                resolve: null//context => queries.GetHrProduct(context.Source.Id)
                );
        }
    }
}
