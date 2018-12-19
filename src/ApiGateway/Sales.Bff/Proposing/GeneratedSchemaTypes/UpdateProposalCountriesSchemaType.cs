using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdateProposalCountriesSchemaType: InputObjectGraphType<UpdateProposalCountriesCommand>
	{
		public UpdateProposalCountriesSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "UpdateProposalCountries";
			Field(x => x.Countries, nullable: true, type:typeof(ListGraphType<ProposalCountryInputSchemaType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
