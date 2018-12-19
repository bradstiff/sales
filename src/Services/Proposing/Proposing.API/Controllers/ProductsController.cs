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
        [ProducesResponseType(typeof(List<ProductDefinitionViewModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetProducts()
        {
            var payroll = new ProductDefinitionViewModel
            {
                Id = 1,
                Name = "Payroll"
            };
            return Ok(new[]
            {
                payroll,
                new ProductDefinitionViewModel
                {
                    Id = 4,
                    Name = "HR",
                    DependsOnProducts = new [] { payroll }.ToList()
                },
                new ProductDefinitionViewModel
                {
                    Id = 64,
                    Name = "Time"
                }
            });
        }

        [Route("components")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ComponentViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetComponents()
        {
            var components = await _proposingQueries.GetComponentsAsync();
            return Ok(components);
        }
    }
}