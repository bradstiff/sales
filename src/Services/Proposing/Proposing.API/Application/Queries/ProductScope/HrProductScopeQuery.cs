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
    public class HrProductViewModel
    {
        public int ProposalId { get; set; }
        public int LevelId { get; set; }
        public IEnumerable<HrProductCountryViewModel> Countries { get; set; }
    }

    public class HrProductQuery : ProductScopeQueryBase<HrProductViewModel> { }

    public class HrProductQueryHandler: IRequestHandler<HrProductQuery, HrProductViewModel>
    {
        private readonly QueryConnectionFactory _connectionFactory;

        public HrProductQueryHandler(QueryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<HrProductViewModel> Handle(HrProductQuery request, CancellationToken cancellationToken)
        {
            using (var conn = _connectionFactory.Create())
            {
                var result = await conn.QueryFirstAsync<HrProductViewModel>(@"
                    select * from HrProduct where ProposalId = @proposalId",
                new { proposalId = request.ProposalId });
                return result;
            }
        }
    }
}
