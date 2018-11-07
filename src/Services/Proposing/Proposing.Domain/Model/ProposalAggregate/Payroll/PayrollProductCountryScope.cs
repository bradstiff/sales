using Proposing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate
{
    public class PayrollProductCountryScope: ValueObject
    {
        public PayrollProductCountryScope()
        {
        }

        public PayrollProductCountryScope(short? levelId, int? weeklyPayees, int? biWeeklyPayees, int? semiMonthlyPayees, int? monthlyPayees, bool? reporting, bool? payslipStorage)
        {
            LevelId = levelId;
            WeeklyPayees = weeklyPayees;
            BiWeeklyPayees = biWeeklyPayees;
            SemiMonthlyPayees = semiMonthlyPayees;
            MonthlyPayees = monthlyPayees;
            Reporting = reporting;
            PayslipStorage = payslipStorage;
        }

        public short? LevelId { get; private set; }
        public int? WeeklyPayees { get; private set; }
        public int? BiWeeklyPayees { get; private set; }
        public int? SemiMonthlyPayees { get; private set; }
        public int? MonthlyPayees { get; private set; }
        public bool? Reporting { get; private set; }
        public bool? PayslipStorage { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return LevelId;
        }
    }
}
