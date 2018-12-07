
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProposalListPageSchemaType: ObjectGraphType<ListPageViewModelOfProposalViewModel>
	{
		public ProposalListPageSchemaType(ProposingClient client)
		{
			Name = "ProposalListPage";
			Field(x => x.TotalCount, nullable: true);
			Field(x => x.Page, nullable: true);
			Field(x => x.Rows, nullable: true, type:typeof(ListGraphType<ProposalSchemaType>));
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
