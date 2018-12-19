using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proposing.API.Application.Commands;
using Proposing.API.Application.Commands.UpdateProductScope;
using Proposing.API.Application.Queries;

namespace Proposing.API.Controllers
{
    [Route("api/[controller]/{proposalId:int}")]
    [ApiController]
    public class HrProductScopeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ProposingQueries _proposingQueries;

        public HrProductScopeController(IMediator mediator, ProposingQueries proposingQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _proposingQueries = proposingQueries ?? throw new ArgumentNullException(nameof(proposingQueries));
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(HrProductViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetGlobalScope(int proposalId)
        {
            var result = await _proposingQueries.GetHrProductAsync(proposalId);
            return Ok(result);
        }

        [Route("countries")]
        [HttpGet]
        [ProducesResponseType(typeof(List<HrProductCountryViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCountryScope(int proposalId, [FromQuery(Name = "countryId")] int[] countryIds)
        {
            var result = await _proposingQueries.GetHrProductCountryAsync(proposalId, countryIds);
            return Ok(result);
        }

        [Route("")]
        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateScope(int proposalId, [FromBody] UpdateHrProductScopeCommand command)
        {
            var result = await _mediator.Send(new CommandWithResourceId<int, UpdateHrProductScopeCommand, bool>(proposalId, command));
            if (!result)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}