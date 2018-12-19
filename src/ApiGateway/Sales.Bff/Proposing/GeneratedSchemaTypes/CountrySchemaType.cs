using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class CountrySchemaType: ObjectGraphType<CountryViewModel>
	{
		public CountrySchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "Country";
			Field(x => x.Id, nullable: false);
			Field(x => x.Name, nullable: true);
			Field(x => x.IsoCode, nullable: true);
			Field(x => x.RegionId, nullable: true);
			Field(x => x.RegionName, nullable: true);
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
