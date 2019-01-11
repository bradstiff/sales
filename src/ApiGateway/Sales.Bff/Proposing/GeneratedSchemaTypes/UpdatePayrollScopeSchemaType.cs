using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdatePayrollScopeSchemaType: InputObjectGraphType<UpdatePayrollScopeCommand>
	{
		public UpdatePayrollScopeSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "UpdatePayrollScope";
			Field(x => x.CountryScopes, nullable: true, type:typeof(ListGraphType<UpdatePayrollCountryScopeSchemaType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
