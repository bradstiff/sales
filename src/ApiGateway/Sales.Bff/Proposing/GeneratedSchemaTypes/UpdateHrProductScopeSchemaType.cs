using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdateHrProductScopeSchemaType: InputObjectGraphType<UpdateHrProductScopeCommand>
	{
		public UpdateHrProductScopeSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "UpdateHrProductScope";
			Field(x => x.LevelId, nullable: true);
			Field(x => x.CountryIds, nullable: true, type:typeof(ListGraphType<IntGraphType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
