using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate.Payroll
{
    public class PayrollProductScopeDto : ProductScopeDto
    {
        public bool Reporting { get; set; }
        public bool PayslipStorage { get; set; }
    }

    public class PayrollProductCountryScopeDto : ProductCountryScopeDto
    {
    }
}
