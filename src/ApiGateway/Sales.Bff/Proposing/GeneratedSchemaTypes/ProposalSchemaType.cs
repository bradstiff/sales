using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProposalSchemaType: ObjectGraphType<ProposalViewModel>
	{
		public ProposalSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "Proposal";
			Field(x => x.Id, nullable: false);
			Field(x => x.Name, nullable: true);
			Field(x => x.ClientName, nullable: true);
			Field(x => x.Comments, nullable: true);
			Field(x => x.Countries, nullable: true, type:typeof(ListGraphType<ProposalCountrySchemaType>));
			Field(x => x.ProductModelId, nullable: true);
			Field(x => x.ProductIds, nullable: true);
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
