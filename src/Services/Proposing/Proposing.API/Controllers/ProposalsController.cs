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
using Proposing.API.Application.Exceptions;
using Proposing.API.Application.Queries;

namespace Proposing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ProposingQueries _proposingQueries;

        public ProposalsController(IMediator mediator, ProposingQueries proposingQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _proposingQueries = proposingQueries ?? throw new ArgumentNullException(nameof(proposingQueries));
        }

        [Route("{proposalId:int}", Name = "GetProposal")]
        [HttpGet]
        [ProducesResponseType(typeof(ProposalViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProposal(int proposalId)
        {
            var proposal = await _proposingQueries.GetProposalAsync(proposalId);
            return Ok(proposal);
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(ListPageViewModel<ProposalViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetProposals(int offset, int limit)
        {
            var proposals = await _proposingQueries.GetProposalListAsync(offset, limit);
            return Ok(proposals);
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(ProposalViewModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateProposal([FromBody] CreateProposalCommand command)
        {
            var proposalId = await _mediator.Send(command);
            if (proposalId <= 0)
            {
                return BadRequest();
            }
            var proposal = await _proposingQueries.GetProposalAsync(proposalId);
            return Created(Url.Link("GetProposal", new { proposalId }), proposal);
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
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateCountries(int proposalId, [FromBody] UpdateProposalCountriesCommand command)
        {
            var result = await _mediator.Send(new CommandWithResourceId<int, UpdateProposalCountriesCommand, bool>(proposalId, command));
            if (!result)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [Route("{proposalId:int}/hr")]
        [HttpGet]
        [ProducesResponseType(typeof(HrProductViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetHrProductScope(int proposalId)
        {
            var result = await _proposingQueries.GetHrProductAsync(proposalId);
            return Ok(result);
        }

        [Route("{proposalId:int}/hr")]
        [HttpPut]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateHrProductScope(int proposalId, [FromBody] UpdateHrProductScopeCommand command)
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