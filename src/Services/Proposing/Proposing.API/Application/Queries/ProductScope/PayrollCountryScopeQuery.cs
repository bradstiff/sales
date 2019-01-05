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
    public class PayrollCountryScopeQuery : ProductCountryScopeQueryBase<PayrollCountryScopeViewModel> { }

    public class PayrollProductCountryQueryHandler : IRequestHandler<PayrollCountryScopeQuery, List<PayrollCountryScopeViewModel>>
    {
        private readonly QueryConnectionFactory _connectionFactory;

        public PayrollProductCountryQueryHandler(QueryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<PayrollCountryScopeViewModel>> Handle(PayrollCountryScopeQuery request, CancellationToken cancellationToken)
        {
            using (var conn = _connectionFactory.Create())
            {
                var query = "select * from PayrollCountryScope p join Component c on p.LevelId = c.Id where p.ProposalId = @proposalId";
                if (request.CountryIds?.Count > 0)
                {
                    query += " and p.CountryId in @countryIds";
                }
                var parameters = new
                {
                    proposalId = request.ProposalId,
                    countryIds = request.CountryIds
                };
                var results = await conn.QueryAsync<PayrollCountryScopeViewModel, ComponentViewModel, PayrollCountryScopeViewModel>(
                    query, 
                    (product, component) => { product.Level = component; return product; },
                    parameters);
                return results.ToList();
            }
        }
    }
}
