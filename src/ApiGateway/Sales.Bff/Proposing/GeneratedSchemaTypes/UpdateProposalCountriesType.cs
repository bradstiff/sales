
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdateProposalCountriesType: InputObjectGraphType<UpdateProposalCountriesCommand>
	{
		public UpdateProposalCountriesType(ProposingClient client)
		{
			Name = "UpdateProposalCountries";
			Field(x => x.Countries, nullable: true, type:typeof(ListGraphType<ProposalCountryInputType>));
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
