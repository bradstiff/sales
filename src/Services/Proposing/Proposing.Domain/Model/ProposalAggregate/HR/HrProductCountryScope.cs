using Proposing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate
{
    public class HrProductCountryScope : ValueObject
    {
        public short? LevelId { get; private set; }

        public HrProductCountryScope()
        {
        }

        public HrProductCountryScope(short? levelId)
        {
            LevelId = levelId;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return LevelId;
        }
    }
}
