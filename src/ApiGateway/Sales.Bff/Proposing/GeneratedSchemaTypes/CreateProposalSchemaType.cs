using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class CreateProposalSchemaType: InputObjectGraphType<CreateProposalCommand>
	{
		public CreateProposalSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "CreateProposal";
			Field(x => x.Name, nullable: false);
			Field(x => x.ClientName, nullable: false);
			Field(x => x.Comments, nullable: true);
			Field(x => x.Countries, nullable: false, type:typeof(ListGraphType<ProposalCountryInputSchemaType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
