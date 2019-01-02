using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProductModelSchemaType: ObjectGraphType<ProductModelViewModel>
	{
		public ProductModelSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "ProductModel";
			Field(x => x.Id, nullable: false);
			Field(x => x.Name, nullable: true);
			Field(x => x.Products, nullable: true, type:typeof(ListGraphType<ProductDefinitionSchemaType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
