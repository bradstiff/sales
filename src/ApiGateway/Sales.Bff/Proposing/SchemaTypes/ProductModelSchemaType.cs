using GraphQL.Types;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public class ProductModelSchemaType : ObjectGraphType
    {
        public ProductModelSchemaType(ProposingClient client)
        {
            Name = "ProductModel";
            Field<ListGraphType<ProductDefinitionSchemaType>>(
                "Products",
                resolve: context => client.Products_GetProductsAsync());
            Field<PayrollProductModelSchemaType>(
                "PayrollProductModel",
                resolve: context => PayrollProductModelSchemaType.Resolve((IEnumerable<ComponentViewModel>)context.Source)
            );
            Field<HrProductModelSchemaType>(
                "HrProductModel", 
                resolve: context => HrProductModelSchemaType.Resolve((IEnumerable<ComponentViewModel>)context.Source)
            );
        }
    }
}
