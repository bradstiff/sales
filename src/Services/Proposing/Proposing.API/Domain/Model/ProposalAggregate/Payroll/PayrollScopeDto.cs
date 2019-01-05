using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate.Payroll
{
    public class PayrollScopeDto : ProductScopeDto
    {
        public bool Reporting { get; set; }
        public bool PayslipStorage { get; set; }
    }

    public class PayrollCountryScopeDto : ProductCountryScopeDto
    {
    }
}
