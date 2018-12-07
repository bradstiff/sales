
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProposalCountrySchemaType: ObjectGraphType<ProposalCountryViewModel>
	{
		public ProposalCountrySchemaType(ProposingClient client)
		{
			Name = "ProposalCountry";
			Field(x => x.Id, nullable: false);
			Field(x => x.CountryId, nullable: true);
			Field(x => x.Name, nullable: true);
			Field(x => x.Headcount, nullable: true);
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
