
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProposalCountryType: ObjectGraphType<ProposalCountryViewModel>
	{
		public ProposalCountryType(ProposingClient client)
		{
			Name = "ProposalCountry";
			Field(x => x.Id, nullable: true, type:(typeof(IdGraphType)));
			Field(x => x.CountryId, nullable: true);
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
