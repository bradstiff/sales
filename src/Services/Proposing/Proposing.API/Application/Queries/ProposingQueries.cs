using Dapper;
using Proposing.API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class ProposingQueries
    {
        private readonly QueryConnectionFactory _connectionFactory;

        public ProposingQueries(QueryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<ProposalViewModel> GetProposalAsync(int id)
        {
            using (var conn = _connectionFactory.Create())
            {
                var results = await conn.QueryMultipleAsync(@"
                    select * from Proposal where Id = @id;
                    select pc.*, c.Name from ProposalCountry pc join Country c on pc.CountryID = c.Id where ProposalId = @id order by c.Name"
                    , new { id });
                var proposal = await results.ReadSingleAsync<ProposalViewModel>();
                proposal.Countries = await results.ReadAsync<ProposalCountryViewModel>();
                return proposal;
            }
        }

        public async Task<ListPageViewModel<ProposalViewModel>> GetProposalListAsync(int page = 1, int rowsPerPage = 25, string orderBy = "ClientName", string order = "asc")
        {
            var offset = (page - 1) * rowsPerPage;
            if (offset < 0) offset = 0;
            var predicates = new List<string>();
            var parameters = new Dictionary<string, object>{
                { "offset", offset },
                { "limit", rowsPerPage } };
            //foreach(var argument in arguments)
            //{
            //    if (argument.Key.Equals("hasCountry", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        predicates.Add($"EXISTS(select 1 from ProposalCountry pc where pc.ProposalID = p.Id and pc.CountryID = @{argument.Key})");
            //    }
            //    else if (argument.Key.Equals("hasProduct", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        predicates.Add($"(ProductTypeIds & @{argument.Key}) = @{argument.Key}");
            //    }
            //    else if (argument.Key.Equals("hasAnyProduct", StringComparison.CurrentCultureIgnoreCase))
            //    {
            //        predicates.Add($"(ProductTypeIds & @{argument.Key}) > 0");
            //    }
            //}

            var countQuery = $@"
            select count(*) from Proposal p 
            {(predicates.Count > 0 ? " WHERE " + string.Join(" AND ", predicates) : "")}
            ";

            var query = $@"
            select * from Proposal p 
            {(predicates.Count > 0 ? " WHERE " + string.Join(" AND ", predicates) : "")}
            ORDER BY {orderBy} {order}
            OFFSET @offset ROWS
            FETCH NEXT @limit ROWS ONLY
            ";

            using (var conn1 = _connectionFactory.Create())
            using (var conn2 = _connectionFactory.Create())
            {
                var count = conn1.QueryFirstAsync<int>(countQuery, parameters);
                var proposals = conn2.QueryAsync<ProposalViewModel>(query, parameters);
                await Task.WhenAll(count, proposals);
                return new ListPageViewModel<ProposalViewModel>(count.Result, page, proposals.Result.ToList());
            }
        }

        public async Task<List<CountryViewModel>> GetCountriesAsync(string orderBy = "RegionName, Name")
        {
            using (var conn = _connectionFactory.Create())
            {
                var results = await conn.QueryAsync<CountryViewModel>($@"
                    select c.*, r.name as RegionName 
                    from Country c 
                    join Region r on r.Id = c.RegionId
                    order by {orderBy}");
                return results.ToList();
            }
        }

        public async Task<List<ComponentViewModel>> GetComponentsAsync()
        {
            using (var conn = _connectionFactory.Create())
            {
                var results = await conn.QueryAsync<ComponentViewModel>("select * from Component order by SortOrder");
                return results.ToList();
            }
        }
    }
}
