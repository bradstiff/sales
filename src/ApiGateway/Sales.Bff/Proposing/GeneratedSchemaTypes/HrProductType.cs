
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class HrProductType: ObjectGraphType<HrProductViewModel>
	{
		public HrProductType(ProposingClient client)
		{
			Name = "HrProduct";
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.LevelId, nullable: true);
			Field(x => x.Countries, nullable: true, type:typeof(ListGraphType<HrProductCountryType>));
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
