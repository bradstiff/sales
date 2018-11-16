
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdateProposalCountriesType: InputObjectGraphType<UpdateProposalCountriesCommand>
	{
		public UpdateProposalCountriesType(ProposingClient client)
		{
			Name = "UpdateProposalCountries";
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.CountryIds, nullable: true, type:typeof(ListGraphType<IntGraphType>));
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
