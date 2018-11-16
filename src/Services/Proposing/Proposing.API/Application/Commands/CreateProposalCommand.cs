using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public CreateProposalCommand(string name, string clientName, string comments, List<int> countryIds)
        {
            Name = name;
            ClientName = clientName;
            Comments = comments;
            CountryIds = countryIds;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ClientName { get; set; }

        public string Comments { get; set; }

        [Required]
        public List<int> CountryIds { get; set; }
    }
}
