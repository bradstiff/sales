using Proposing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate
{
    public class PayrollProductGlobalScope : ValueObject
    {
        public bool? Reporting { get; private set; }
        public bool? PayslipStorage { get; private set; }

        public PayrollProductGlobalScope()
        {
        }

        public PayrollProductGlobalScope(bool reporting, bool payslipStorage)
        {
            Reporting = reporting;
            PayslipStorage = payslipStorage;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Reporting;
            yield return PayslipStorage;
        }
    }
}
