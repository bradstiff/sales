using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proposing.API.Application.Queries;

namespace Proposing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ProposingQueries _proposingQueries;

        public ProductsController(IMediator mediator, ProposingQueries proposingQueries)
        {
            _mediator = mediator;
            _proposingQueries = proposingQueries;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ComponentViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetComponents(short componentTypeId)
        {
            var components = await _proposingQueries.GetComponentsAsync(componentTypeId);
            return Ok(components);
        }
    }
}