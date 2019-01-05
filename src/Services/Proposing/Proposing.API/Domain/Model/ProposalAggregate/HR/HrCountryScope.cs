using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public class HrCountryScope : Entity, IProductCountryScope
    {
        public int CountryId { get; private set; }
        public short? LevelId { get; private set; }

        public HrCountryScope()
        {
        }

        public HrCountryScope(int countryId)
        {
            this.CountryId = countryId;
        }

        public void SetScopeValues(short levelId)
        {
            this.LevelId = levelId;
        }
    }
}
