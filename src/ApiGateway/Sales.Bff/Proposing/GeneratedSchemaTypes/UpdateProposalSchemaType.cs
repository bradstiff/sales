using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdateProposalSchemaType: InputObjectGraphType<UpdateProposalCommand>
	{
		public UpdateProposalSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "UpdateProposal";
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.Name, nullable: true);
			Field(x => x.ClientName, nullable: true);
			Field(x => x.Comments, nullable: true);
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
