using GraphQL.Bff.Proposing;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class Proposal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
    }
}