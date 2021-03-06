﻿using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{

    public partial class ProposalSchemaType : ObjectGraphType<ProposalViewModel>
    {
        partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
        {
            Field<ProductModelSchemaType>(
                "productModel",
                resolve: context => cache.ProductModels[context.Source.ProductModelId]
            );
            Field<ListGraphType<ProductDefinitionSchemaType>>(
                "products",
                resolve: context =>
                {
                    var model = cache.ProductModels[context.Source.ProductModelId];
                    return context.Source.ProductIds
                        .GetBits()
                        .Select(id => model.Products.First(p => p.Id == id));
                }
            );
            Field<PayrollScopeSchemaType>(
                "payrollScope",
                resolve: context => client.PayrollScope_GetGlobalScopeAsync(context.Source.Id)
            );
            Field<HrScopeSchemaType>(
                "hrScope",
                resolve: context => client.HrScope_GetGlobalScopeAsync(context.Source.Id)
            );
        }
    }
}
