
using GraphQL.Types;
using Proposing.API.Client;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class UpdateHrProductScopeType: InputObjectGraphType<UpdateHrProductScopeCommand>
	{
		public UpdateHrProductScopeType(ProposingClient client)
		{
			Name = "UpdateHrProductScope";
			Field(x => x.LevelId, nullable: true);
			Field(x => x.CountryIds, nullable: true, type:typeof(ListGraphType<IntGraphType>));
			this.Extend(client);
		}

		partial void Extend(ProposingClient client);
	}
}
