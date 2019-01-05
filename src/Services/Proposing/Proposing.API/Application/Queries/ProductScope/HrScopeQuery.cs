using Dapper;
using MediatR;
using Proposing.API.Application.Queries.ProductModel;
using Proposing.API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductScope
{
    public class HrScopeQuery : ProductScopeQueryBase<HrScopeViewModel> { }

    public class HrProductQueryHandler: IRequestHandler<HrScopeQuery, HrScopeViewModel>
    {
        private readonly QueryConnectionFactory _connectionFactory;

        public HrProductQueryHandler(QueryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<HrScopeViewModel> Handle(HrScopeQuery request, CancellationToken cancellationToken)
        {
            using (var conn = _connectionFactory.Create())
            {
                var query = "select * from HrScope p join Component c on p.LevelId = c.Id where p.ProposalId = @proposalId";
                var result = await conn.QueryAsync<HrScopeViewModel, ComponentViewModel, HrScopeViewModel>(
                    query,
                    (product, component) => { product.Level = component; return product; },
                    request);
                return result.SingleOrDefault();
            }
        }
    }
}
