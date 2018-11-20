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

        public CreateProposalCommand(string name, string clientName, string comments, List<ProposalCountryDto> countries)
        {
            Name = name;
            ClientName = clientName;
            Comments = comments;
            Countries = countries;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ClientName { get; set; }

        public string Comments { get; set; }

        [Required]
        public List<ProposalCountryDto> Countries { get; set; }
    }

    public class ProposalCountryDto
    {
        public ProposalCountryDto()
        {
        }

        public ProposalCountryDto(int countryId, int? headcount)
        {
            CountryId = countryId;
            Headcount = headcount;
        }

        public int CountryId { get; set; }

        public int? Headcount { get; set; }
    }
}
