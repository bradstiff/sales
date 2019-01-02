using GraphQL.DataLoader;
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
                    var bitArray = new BitArray(BitConverter.GetBytes(context.Source.ProductIds));
                    var products = new List<ProductDefinitionViewModel>();
                    for (int i = 0; i < bitArray.Length; i++)
                    {
                        if (bitArray[i])
                        {
                            products.Add(model.Products.First(p => p.Id == (2 ^ i)));
                        }
                    }
                    return products;
                }
            );
            Field<HrProductSchemaType>(
                "hr",
                resolve: context => client.HrProductScope_GetGlobalScopeAsync(context.Source.Id)
            );
        }
    }
}
