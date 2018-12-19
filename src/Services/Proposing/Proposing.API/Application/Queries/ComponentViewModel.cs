using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class ComponentViewModel
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public short SortOrder { get; set; }
        public int? ProductId { get; set; }
        public short? ComponentTypeID { get; set; }
    }
}
