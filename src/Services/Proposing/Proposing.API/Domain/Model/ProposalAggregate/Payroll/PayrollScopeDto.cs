using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate.Payroll
{
    public class PayrollScopeDto : ProductScopeDto<PayrollCountryScopeDto>
    {
        public bool Reporting { get; set; }
        public bool PayslipStorage { get; set; }
    }

    public class PayrollCountryScopeDto : ProductCountryScopeDto
    {
        public short LevelId { get; set; }
        public int WeeklyPayees { get; set; }
        public int BiWeeklyPayees { get; set; }
        public int SemiMonthlyPayees { get; set; }
        public int MonthlyPayees { get; set; }
        public bool? Reporting { get; set; }
        public bool? PayslipStorage { get; set; }
    }
}
