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
    public class CountriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ProposingQueries _proposingQueries;

        public CountriesController(IMediator mediator, ProposingQueries proposingQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _proposingQueries = proposingQueries ?? throw new ArgumentNullException(nameof(proposingQueries));
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(List<CountryViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _proposingQueries.GetCountries();
            return Ok(countries);
        }
    }
}