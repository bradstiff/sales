using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ComponentSchemaType: ObjectGraphType<ComponentViewModel>
	{
		public ComponentSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "Component";
			Field(x => x.Id, nullable: false);
			Field(x => x.Name, nullable: true);
			Field(x => x.FullName, nullable: true);
			Field(x => x.IsActive, nullable: true);
			Field(x => x.SortOrder, nullable: true);
			Field(x => x.ProductId, nullable: true);
			Field(x => x.ComponentTypeID, nullable: true);
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
