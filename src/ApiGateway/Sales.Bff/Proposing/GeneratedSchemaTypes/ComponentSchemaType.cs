
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ComponentSchemaType: ObjectGraphType<ComponentViewModel>
	{
		public ComponentSchemaType(ProposingClient client)
		{
			Name = "Component";
			Field(x => x.Id, nullable: false);
			Field(x => x.Name, nullable: true);
			Field(x => x.FullName, nullable: true);
			Field(x => x.IsActive, nullable: true);
			Field(x => x.SortOrder, nullable: true);
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
