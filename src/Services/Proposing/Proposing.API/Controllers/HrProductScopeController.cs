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
using Proposing.API.Application.Queries.ProductScope;

namespace Proposing.API.Controllers
{
    [Route("api/[controller]/{proposalId:int}")]
    [ApiController]
    public class HrProductScopeController : ProductScopeController<HrProductQuery, HrProductViewModel, HrProductCountryQuery, HrProductCountryViewModel, UpdateHrProductScopeCommand>
    {
        public HrProductScopeController(IMediator mediator) : base(mediator)
        {
        }
    }
}