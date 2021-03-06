﻿using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public class PayrollCountryScope : Entity, IProductCountryScope
    {
        public PayrollCountryScope()
        {
        }

        public PayrollCountryScope(int countryId)
        {
            this.CountryId = countryId;
        }

        public int CountryId { get; private set; }
        public short LevelId { get; private set; }
        public int WeeklyPayees { get; private set; }
        public int BiWeeklyPayees { get; private set; }
        public int SemiMonthlyPayees { get; private set; }
        public int MonthlyPayees { get; private set; }
        public bool? Reporting { get; private set; }
        public bool? PayslipStorage { get; private set; }

        public void SetScopeValues(short levelId, int weeklyPayees, int biWeeklyPayees, int semiMonthlyPayees, int monthlyPayees, bool? reporting, bool? payslipStorage)
        {
            LevelId = levelId;
            WeeklyPayees = weeklyPayees;
            BiWeeklyPayees = biWeeklyPayees;
            SemiMonthlyPayees = semiMonthlyPayees;
            MonthlyPayees = monthlyPayees;
            Reporting = reporting;
            PayslipStorage = payslipStorage;
        }
    }
}
