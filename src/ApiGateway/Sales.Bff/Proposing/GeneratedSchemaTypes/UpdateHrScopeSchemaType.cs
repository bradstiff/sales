using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdateHrScopeSchemaType: InputObjectGraphType<UpdateHrScopeCommand>
	{
		public UpdateHrScopeSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "UpdateHrScope";
			Field(x => x.LevelId, nullable: true);
			Field(x => x.CountryIds, nullable: true, type:typeof(ListGraphType<IntGraphType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
