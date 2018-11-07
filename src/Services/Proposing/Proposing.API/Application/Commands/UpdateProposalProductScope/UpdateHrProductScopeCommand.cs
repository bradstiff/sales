using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands.UpdateProposalProductScope
{
    public class UpdateHrProductScopeCommand : IRequest<bool>
    {
        private readonly List<HrCountryScopeDTO> _countryScopes;

        public UpdateHrProductScopeCommand()
        {
            _countryScopes = new List<HrCountryScopeDTO>();
        }

        public UpdateHrProductScopeCommand(List<HrCountryScopeDTO> countryScopes, short levelId)
        {
            _countryScopes = countryScopes;
            LevelId = levelId;
        }

        public short LevelId { get; set; }

        public IReadOnlyCollection<HrCountryScopeDTO> CountryScopes => _countryScopes;

        public class HrCountryScopeDTO
        {
            public int CountryId { get; set; }
        }
    }
}
