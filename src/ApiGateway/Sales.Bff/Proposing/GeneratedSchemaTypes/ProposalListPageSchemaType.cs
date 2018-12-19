using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProposalListPageSchemaType: ObjectGraphType<ListPageViewModelOfProposalViewModel>
	{
		public ProposalListPageSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "ProposalListPage";
			Field(x => x.TotalCount, nullable: true);
			Field(x => x.Page, nullable: true);
			Field(x => x.Rows, nullable: true, type:typeof(ListGraphType<ProposalSchemaType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
