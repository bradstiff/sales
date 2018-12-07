
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProposalCountryInputSchemaType: InputObjectGraphType<ProposalCountryDto>
	{
		public ProposalCountryInputSchemaType(ProposingClient client)
		{
			Name = "ProposalCountryInput";
			Field(x => x.CountryId, nullable: true);
			Field(x => x.Headcount, nullable: true);
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
