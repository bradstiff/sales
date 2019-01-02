using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proposing.API.Application.Queries;
using Proposing.API.Application.Queries.ProductModel;

namespace Proposing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductModelsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ProposingQueries _proposingQueries;

        public ProductModelsController(IMediator mediator, ProposingQueries proposingQueries)
        {
            _mediator = mediator;
            _proposingQueries = proposingQueries;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<ProductModelViewModel>>> GetAllProductModels()
        {
            return Ok(await this.GetModels());
        }

        [Route("{productModelId:int}")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductModelViewModel>> GetProductModel(int id)
        {
            var model = (await this.GetModels()).Single(m => m.Id == id);
            return Ok(model);
        }

        private async Task<List<ProductModelViewModel>> GetModels()
        {
            var components = await _proposingQueries.GetComponentsAsync();
            var payroll = new ProductDefinitionViewModel
            {
                Id = 1,
                Name = "Payroll",
                Levels = components.Where(c => c.ComponentTypeID == 2).ToList(),
                Components = components.Where(c => c.ProductId == 1).ToList()
            };
            var hr = new ProductDefinitionViewModel
            {
                Id = 4,
                Name = "HR",
                Levels = components.Where(c => c.ComponentTypeID == 10).ToList(),
                Components = components.Where(c => c.ProductId == 4).ToList()
            };
            var time = new ProductDefinitionViewModel
            {
                Id = 64,
                Name = "Time"
            };
            //todo: hard-coded, levels and dependencies by model version
            return new List<ProductModelViewModel>
            {
                new ProductModelViewModel
                {
                    Id = 1,
                    Name = "Original",
                    Products = new List<ProductDefinitionViewModel>
                    {
                        payroll,
                        hr
                    }
                },
                new ProductModelViewModel
                {
                    Id = 2,
                    Name = "Time",
                    Products = new List<ProductDefinitionViewModel>
                    {
                        payroll,
                        hr,
                        time
                    }
                },
            };
        }
    }
}