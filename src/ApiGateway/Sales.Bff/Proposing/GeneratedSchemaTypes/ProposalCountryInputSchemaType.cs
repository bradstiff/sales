using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProposalCountryInputSchemaType: InputObjectGraphType<ProposalCountryDto>
	{
		public ProposalCountryInputSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "ProposalCountryInput";
			Field(x => x.CountryId, nullable: true);
			Field(x => x.Headcount, nullable: true);
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
