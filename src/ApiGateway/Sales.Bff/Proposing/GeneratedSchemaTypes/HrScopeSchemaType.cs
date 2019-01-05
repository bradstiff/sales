using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class HrScopeSchemaType: ObjectGraphType<HrScopeViewModel>
	{
		public HrScopeSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "HrScope";
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.Level, nullable: true, type:typeof(ComponentSchemaType));
			Field(x => x.Countries, nullable: true, type:typeof(ListGraphType<HrCountryScopeSchemaType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
