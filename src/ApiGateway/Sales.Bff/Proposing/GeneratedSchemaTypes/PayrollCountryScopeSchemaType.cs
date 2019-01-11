using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;

namespace Sales.Bff.Proposing.SchemaTypes
{
	public partial class PayrollCountryScopeSchemaType: ObjectGraphType<PayrollCountryScopeViewModel>
	{
		public PayrollCountryScopeSchemaType(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
		{
			Name = "PayrollCountryScope";
			Field(x => x.ProposalId, nullable: true);
			Field(x => x.CountryId, nullable: true);
			Field(x => x.Level, nullable: true, type:typeof(ComponentSchemaType));
			Field(x => x.MonthlyPayees, nullable: true);
			Field(x => x.SemiMonthlyPayees, nullable: true);
			Field(x => x.BiWeeklyPayees, nullable: true);
			Field(x => x.WeeklyPayees, nullable: true);
			Field(x => x.Reporting, nullable: true);
			Field(x => x.PayslipStorage, nullable: true);
			this.Extend(client, cache, accessor);
		}

		partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor);
	}
}
