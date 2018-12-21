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
    public class PayrollProductCountryQuery : ProductCountryScopeQueryBase<PayrollProductCountryViewModel> { }

    public class PayrollProductCountryQueryHandler : IRequestHandler<PayrollProductCountryQuery, List<PayrollProductCountryViewModel>>
    {
        private readonly QueryConnectionFactory _connectionFactory;

        public PayrollProductCountryQueryHandler(QueryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<PayrollProductCountryViewModel>> Handle(PayrollProductCountryQuery request, CancellationToken cancellationToken)
        {
            using (var conn = _connectionFactory.Create())
            {
                var query = "select * from PayrollProductCountry where ProposalId = @proposalId";
                if (request.CountryIds?.Count > 0)
                {
                    query += " and CountryId in @countryIds";
                }
                var parameters = new
                {
                    proposalId = request.ProposalId,
                    countryIds = request.CountryIds
                };
                var results = await conn.QueryAsync<PayrollProductCountryViewModel>(query, parameters);
                return results.ToList();
            }
        }
    }
}
