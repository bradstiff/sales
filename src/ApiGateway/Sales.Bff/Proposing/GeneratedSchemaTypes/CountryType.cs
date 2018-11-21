
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class CountryType: ObjectGraphType<CountryViewModel>
	{
		public CountryType(ProposingClient client)
		{
			Name = "Country";
			Field(x => x.Id, nullable: false);
			Field(x => x.Name, nullable: true);
			Field(x => x.IsoCode, nullable: true);
			Field(x => x.RegionId, nullable: true);
			Field(x => x.RegionName, nullable: true);
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
