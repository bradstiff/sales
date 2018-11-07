using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proposing.API.Application.Commands;
using Proposing.API.Application.Commands.UpdateProposalProductScope;
using Proposing.API.Application.Exceptions;

namespace Proposing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProposalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("{proposalId:int}", Name = "GetProposal")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetProposal(int proposalId)
        {
            return Ok();
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateProposal([FromBody] CreateProposalCommand command)
        {
            var proposalId = await _mediator.Send(command);
            if (proposalId <= 0)
            {
                return BadRequest();
            }
            return Created(Url.Link("GetProposal", new { proposalId }), new { });
        }

        [Route("{proposalId:int}")]
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProposal(int proposalId, [FromBody] UpdateProposalCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Route("{proposalId:int}/countries")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateCountries(int proposalId, [FromBody] UpdateProposalCountriesCommand command)
        {
            var result = await _mediator.Send(new CommandWithResourceId<int, UpdateProposalCountriesCommand, bool>(proposalId, command));
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Route("{proposalId:int}/hr")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateHrProductScope(int proposalId, [FromBody] UpdateHrProductScopeCommand command)
        {
            var result = await _mediator.Send(new CommandWithResourceId<int, UpdateHrProductScopeCommand, bool>(proposalId, command));
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}