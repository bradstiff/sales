
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class CreateProposalSchemaType: InputObjectGraphType<CreateProposalCommand>
	{
		public CreateProposalSchemaType(ProposingClient client)
		{
			Name = "CreateProposal";
			Field(x => x.Name, nullable: false);
			Field(x => x.ClientName, nullable: false);
			Field(x => x.Comments, nullable: true);
			Field(x => x.Countries, nullable: false, type:typeof(ListGraphType<ProposalCountryInputSchemaType>));
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
