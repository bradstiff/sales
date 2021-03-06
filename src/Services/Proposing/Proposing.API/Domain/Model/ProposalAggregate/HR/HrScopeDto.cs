﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate.HR
{
    public class HrScopeDto: ProductScopeDto<HrCountryScopeDto>
    {
        public short LevelId { get; set; }
    }

    public class HrCountryScopeDto: ProductCountryScopeDto
    {
    }
}
