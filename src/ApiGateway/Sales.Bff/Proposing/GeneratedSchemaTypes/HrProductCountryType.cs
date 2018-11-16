
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class HrProductCountryType: ObjectGraphType<HrProductCountryViewModel>
	{
		public HrProductCountryType(ProposingClient client)
		{
			Name = "HrProductCountry";
			Field(x => x.CountryId, nullable: true);
			Field(x => x.LevelId, nullable: true);
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
