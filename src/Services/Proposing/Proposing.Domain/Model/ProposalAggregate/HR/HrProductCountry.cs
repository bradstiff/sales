using Proposing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate
{
    public class HrProductCountry : Entity, IProductCountry
    {
        public int CountryId { get; private set; }
        public short? LevelId { get; private set; }

        public HrProductCountry()
        {
        }

        public HrProductCountry(int countryId)
        {
            this.CountryId = countryId;
        }

        public void SetScopeValues(short levelId)
        {
            this.LevelId = levelId;
        }
    }
}
