using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductModel
{
    public class ProductDefinitionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ComponentViewModel> Levels { get; set; }
        public List<ComponentViewModel> Components { get; set; }
        public List<ProductDefinitionViewModel> DependsOnProducts { get; set; }
    }
}
