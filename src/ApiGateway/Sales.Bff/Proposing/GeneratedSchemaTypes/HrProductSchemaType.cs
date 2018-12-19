using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class HrProductSchemaType: ObjectGraphType<HrProductViewModel>
	{
		public HrProductSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "HrProduct";
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.LevelId, nullable: true);
			Field(x => x.Countries, nullable: true, type:typeof(ListGraphType<HrProductCountrySchemaType>));
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
