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
    public class PayrollProductScopeController : ProductScopeController<PayrollProductQuery, PayrollProductViewModel, PayrollProductCountryQuery, PayrollProductCountryViewModel, UpdatePayrollProductScopeCommand>
    {
        public PayrollProductScopeController(IMediator mediator) : base(mediator)
        {
        }
    }
}