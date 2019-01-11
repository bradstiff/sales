using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proposing.API.Application.Commands.UpdateProductScope;
using Proposing.API.Application.Queries.ProductScope;

namespace Proposing.API.Controllers
{
    [Route("api/[controller]/{proposalId:int}")]
    [ApiController]
    public class PayrollScopeController : ProductScopeController<PayrollProductQuery, PayrollScopeViewModel, PayrollCountryScopeQuery, PayrollCountryScopeViewModel, UpdatePayrollScopeCommand>
    {
        public PayrollScopeController(IMediator mediator) : base(mediator)
        {
        }
    }
}