using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public class Component
    {
        public short Id { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public bool IsActive { get; private set; }
        public byte SortOrder { get; private set; }
        public ComponentType ComponentType { get; private set; }
    }
}
