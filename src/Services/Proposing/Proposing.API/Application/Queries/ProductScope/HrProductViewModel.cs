﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductScope
{
    public class HrProductViewModel
    {
        public int ProposalId { get; set; }
        public int LevelId { get; set; }
        public IEnumerable<HrProductCountryViewModel> Countries { get; set; }
    }
}
