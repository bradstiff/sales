using Proposing.API.Application.Queries.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductScope
{
    public class PayrollCountryScopeViewModel
    {
        public int ProposalId { get; set; }
        public int CountryId { get; set; }
        public ComponentViewModel Level { get; set; }
        public int MonthlyPayees { get; set; }
        public int SemiMonthlyPayees { get; set; }
        public int BiWeeklyPayees { get; set; }
        public int WeeklyPayees { get; set; }
        public bool? Reporting { get; set; }
        public bool? PayslipStorage { get; set; }

    }
}
