using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class PayrollProductSchemaType: ObjectGraphType<PayrollProductViewModel>
	{
		public PayrollProductSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "PayrollProduct";
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.Countries, nullable: true, type:typeof(ListGraphType<PayrollProductCountrySchemaType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
