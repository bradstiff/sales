using GraphQL.Types;
using Proposing.API.Application.Queries;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Bff.Proposing.SchemaTypes
{
    public class ProposalCountryType : ObjectGraphType<ProposalCountry>
    {
        public ProposalCountryType(ProposalsClient client)
        {
            Name = "ProposalCountry";
            Field(x => x.Id, type: typeof(IdGraphType));
            Field(x => x.CountryId);
        }
    }
}
