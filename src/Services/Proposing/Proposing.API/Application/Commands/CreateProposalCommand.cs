using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands
{
    public class CreateProposalCommand : IRequest<int>
    {
        public CreateProposalCommand()
        {
        }

        public CreateProposalCommand(string name, string clientName, string comments, IEnumerable<int> countryIds)
        {
            Name = name;
            ClientName = clientName;
            Comments = comments;
            CountryIds = countryIds;
        }

        public string Name { get; set; }
        public string ClientName { get; set; }
        public string Comments { get; set; }
        public IEnumerable<int> CountryIds { get; set; }
    }
}
