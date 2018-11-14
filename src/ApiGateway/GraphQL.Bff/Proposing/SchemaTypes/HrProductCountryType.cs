using GraphQL.Types;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public class HrProductCountryType : ObjectGraphType<HrProductCountry>
    {
        public HrProductCountryType(ProposalsClient client)
        {
            Name = "HRProductCountry";
            Field(x => x.CountryId);
            Field(x => x.LevelId);
        }
    }
}
