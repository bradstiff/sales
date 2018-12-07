
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdateProposalCountriesSchemaType: InputObjectGraphType<UpdateProposalCountriesCommand>
	{
		public UpdateProposalCountriesSchemaType(ProposingClient client)
		{
			Name = "UpdateProposalCountries";
			Field(x => x.Countries, nullable: true, type:typeof(ListGraphType<ProposalCountryInputSchemaType>));
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
