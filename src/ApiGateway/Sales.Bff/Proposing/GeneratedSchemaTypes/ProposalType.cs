
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProposalType: ObjectGraphType<ProposalViewModel>
	{
		public ProposalType(ProposingClient client)
		{
			Name = "Proposal";
			Field(x => x.Id, nullable: false);
			Field(x => x.Name, nullable: true);
			Field(x => x.ClientName, nullable: true);
			Field(x => x.Comments, nullable: true);
			Field(x => x.Countries, nullable: true, type:typeof(ListGraphType<ProposalCountryType>));
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
