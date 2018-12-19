using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class ProductDefinitionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductDefinitionViewModel> DependsOnProducts { get; set; }
    }
}
