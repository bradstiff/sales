using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class ProposalCountrySchemaType: ObjectGraphType<ProposalCountryViewModel>
	{
		public ProposalCountrySchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "ProposalCountry";
			Field(x => x.Id, nullable: false);
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.CountryId, nullable: true);
			Field(x => x.Name, nullable: true);
			Field(x => x.ProductModelId, nullable: true);
			Field(x => x.ProductIds, nullable: true);
			Field(x => x.Headcount, nullable: true);
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
