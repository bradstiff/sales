using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands
{
    public class UpdateProposalCommand: IRequest<bool>
    {
        public UpdateProposalCommand()
        {
        }

        public UpdateProposalCommand(int proposalId, string name, string clientName, string comments)
        {
            ProposalId = proposalId;
            Name = name;
            ClientName = clientName;
            Comments = comments;
        }

        public int ProposalId { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public string Comments { get; set; }
    }
}
