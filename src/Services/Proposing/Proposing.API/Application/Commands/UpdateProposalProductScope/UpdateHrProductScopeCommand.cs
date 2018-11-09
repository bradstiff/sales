using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands.UpdateProposalProductScope
{
    public class UpdateHrProductScopeCommand : IRequest<bool>
    {
        public UpdateHrProductScopeCommand()
        {
        }

        public UpdateHrProductScopeCommand(List<int> countryIds, short levelId)
        {
            CountryIds = countryIds;
            LevelId = levelId;
        }

        public short LevelId { get; set; }

        public List<int> CountryIds { get; set; }
    }
}
