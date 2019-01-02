using Proposing.API.Application.Queries.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class ProductModelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProductDefinitionViewModel> Products { get; set; }
    }
}
