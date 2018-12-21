using MediatR;
using Microsoft.AspNetCore.Mvc;
using Proposing.API.Application.Commands;
using Proposing.API.Application.Queries;
using Proposing.API.Application.Queries.ProductScope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Proposing.API.Controllers
{
    [ApiController]
    public class ProductScopeController<TGlobalQuery, TGlobalQueryResult, TCountryQuery, TCountryQueryResult, TUpdateCommand> : ControllerBase
        where TGlobalQuery: ProductScopeQueryBase<TGlobalQueryResult>, new()
        where TCountryQuery: ProductCountryScopeQueryBase<TCountryQueryResult>, new()
        where TUpdateCommand: IRequest<bool>
    {
        protected readonly IMediator _mediator;

        public ProductScopeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<TGlobalQueryResult>> GetGlobalScope(int proposalId)
        {
            var query = new TGlobalQuery();
            query.Init(proposalId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Route("countries")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<List<TCountryQueryResult>>> GetCountryScope(int proposalId, [FromQuery(Name = "countryId")] int[] countryIds)
        {
            var query = new TCountryQuery();
            query.Init(proposalId, countryIds);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [Route("")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<bool>> UpdateScope(int proposalId, [FromBody] TUpdateCommand command)
        {
            var result = await _mediator.Send(new CommandWithResourceId<int, TUpdateCommand, bool>(proposalId, command));
            if (!result)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
