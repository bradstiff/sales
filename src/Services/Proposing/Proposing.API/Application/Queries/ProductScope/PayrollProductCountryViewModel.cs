﻿using Proposing.API.Application.Queries.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductScope
{
    public class PayrollProductCountryViewModel
    {
        public int ProposalId { get; set; }
        public int CountryId { get; set; }
        public ComponentViewModel Level { get; set; }
    }
}
