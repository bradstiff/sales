using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public partial class ProductModelSchemaType : ObjectGraphType<ProductModelViewModel>
    {
        partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
        {
            Field<ProductDefinitionSchemaType>(
                "payroll",
                resolve: context => cache.ProductModels[context.Source.Id].Products.Find(p => p.Id == (long)ProductType.Payroll)
            );
            Field<ProductDefinitionSchemaType>(
                "hr",
                resolve: context => cache.ProductModels[context.Source.Id].Products.Find(p => p.Id == (long)ProductType.Hr) 
            );
        }
    }
}
