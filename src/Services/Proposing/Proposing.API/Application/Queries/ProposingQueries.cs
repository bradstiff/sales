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
                    select * from ProposalCountry where ProposalId = @id"
                    , new { id });
                var proposal = await results.ReadSingleAsync<ProposalViewModel>();
                proposal.Countries = await results.ReadAsync<ProposalCountryViewModel>();
                return proposal;
            }
        }

        public async Task<IEnumerable<ProposalViewModel>> GetProposalsAsync(Dictionary<string, object> arguments)
        {
            var predicates = new List<string>();
            foreach(var argument in arguments)
            {
                if (argument.Key.Equals("hasCountry", StringComparison.CurrentCultureIgnoreCase))
                {
                    predicates.Add($"EXISTS(select 1 from ProposalCountry pc where pc.ProposalID = p.Id and pc.CountryID = @{argument.Key})");
                }
                else if (argument.Key.Equals("hasProduct", StringComparison.CurrentCultureIgnoreCase))
                {
                    predicates.Add($"(ProductTypeIds & @{argument.Key}) = @{argument.Key}");
                }
                else if (argument.Key.Equals("hasAnyProduct", StringComparison.CurrentCultureIgnoreCase))
                {
                    predicates.Add($"(ProductTypeIds & @{argument.Key}) > 0");
                }
            }
            var query = "select * from Proposal p";
            if (predicates.Count > 0)
            {
                query += " WHERE " + string.Join(" AND ", predicates);
            }

            using (var conn = this.NewConnection())
            {
                return await conn.QueryAsync<ProposalViewModel>(query, arguments);
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
