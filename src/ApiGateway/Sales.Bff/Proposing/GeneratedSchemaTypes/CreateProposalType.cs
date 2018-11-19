
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class CreateProposalType: InputObjectGraphType<CreateProposalCommand>
	{
		public CreateProposalType(ProposingClient client)
		{
			Name = "CreateProposal";
			Field(x => x.Name, nullable: false);
			Field(x => x.ClientName, nullable: false);
			Field(x => x.Comments, nullable: true);
			Field(x => x.CountryIds, nullable: false, type:typeof(ListGraphType<IntGraphType>));
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}