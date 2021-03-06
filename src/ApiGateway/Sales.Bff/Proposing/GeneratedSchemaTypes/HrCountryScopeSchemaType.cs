using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class HrCountryScopeSchemaType: ObjectGraphType<HrCountryScopeViewModel>
	{
		public HrCountryScopeSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "HrCountryScope";
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.CountryId, nullable: true);
			Field(x => x.Level, nullable: true, type:typeof(ComponentSchemaType));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
