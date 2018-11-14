﻿using GraphQL.Types;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public class HrProductType : ObjectGraphType<HrProduct>
    {
        public HrProductType(ProposalsClient client)
        {
            Name = "HRProduct";
            Field(x => x.LevelId);
            Field(x => x.Countries, type: typeof(ListGraphType<HrProductCountryType>));
        }
    }
}
