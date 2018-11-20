using Dapper;
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
        private string _connectionString = string.Empty;
        public ProposingQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
        }

        private IDbConnection NewConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<ProposalViewModel> GetProposalAsync(int id)
        {
            using (var conn = this.NewConnection())
            {
                var results = await conn.QueryMultipleAsync(@"
                    select * from Proposal where Id = @id;
                    select pc.*, c.Name from ProposalCountry pc join Country c on pc.CountryID = c.Id where ProposalId = @id"
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

            using (var conn1 = this.NewConnection())
            using (var conn2 = this.NewConnection())
            {
                var count = conn1.QueryFirstAsync<int>(countQuery, parameters);
                var proposals = conn2.QueryAsync<ProposalViewModel>(query, parameters);
                await Task.WhenAll(count, proposals);
                return new ListPageViewModel<ProposalViewModel>(count.Result, page, proposals.Result.ToList());
            }
        }

        public async Task<HrProductViewModel> GetHrProduct(int proposalId)
        {
            using (var conn = this.NewConnection())
            {
                var results = await conn.QueryMultipleAsync(@"
                    select * from HrProduct where ProposalId = @proposalId;
                    select * from HrProductCountry where ProposalId = @proposalId", 
                new { proposalId });
                var hrProduct = await results.ReadSingleAsync<HrProductViewModel>();
                hrProduct.Countries = await results.ReadAsync<HrProductCountryViewModel>();
                return hrProduct;
            }
        }
    }
}
