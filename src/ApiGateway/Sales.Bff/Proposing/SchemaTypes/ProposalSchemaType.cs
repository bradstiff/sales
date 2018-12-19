﻿using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{

    public partial class ProposalSchemaType : ObjectGraphType<ProposalViewModel>
    {
        partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
        {
            Field<HrProductSchemaType>(
                "hr",
                resolve: context => client.HrProductScope_GetGlobalScopeAsync(context.Source.Id)
            );
        }
    }
}