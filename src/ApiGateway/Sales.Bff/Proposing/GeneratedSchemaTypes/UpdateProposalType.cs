
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdateProposalType: InputObjectGraphType<UpdateProposalCommand>
	{
		public UpdateProposalType(ProposingClient client)
		{
			Name = "UpdateProposal";
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.Name, nullable: true);
			Field(x => x.ClientName, nullable: true);
			Field(x => x.Comments, nullable: true);
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
