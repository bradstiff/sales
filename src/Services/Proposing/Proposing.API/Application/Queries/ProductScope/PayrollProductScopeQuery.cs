using Dapper;
using MediatR;
using Proposing.API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductScope
{
    public class PayrollProductQuery : ProductScopeQueryBase<PayrollProductViewModel> { }

    public class PayrollProductQueryHandler: IRequestHandler<PayrollProductQuery, PayrollProductViewModel>
    {
        private readonly QueryConnectionFactory _connectionFactory;

        public PayrollProductQueryHandler(QueryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PayrollProductViewModel> Handle(PayrollProductQuery request, CancellationToken cancellationToken)
        {
            using (var conn = _connectionFactory.Create())
            {
                var result = await conn.QuerySingleOrDefaultAsync<PayrollProductViewModel>("select * from PayrollProduct where ProposalId = @proposalId", request);
                return result;
            }
        }
    }
}
