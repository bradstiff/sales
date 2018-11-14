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

        public async Task<Proposal> GetProposalAsync(int id)
        {
            using (var conn = this.NewConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<Proposal>(
                        @"select *
                        from Proposal
                        where Id = @id"
                    , new { id });
            }
        }

        public async Task<IEnumerable<Proposal>> GetProposalsAsync(Dictionary<string, object> arguments)
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
                return await conn.QueryAsync<Proposal>(query, arguments);
            }
        }

        public async Task<HrProduct> GetHrProduct(int proposalId)
        {
            using (var conn = this.NewConnection())
            {
                var results = await conn.QueryMultipleAsync(@"
                    select * from HrProduct where ProposalId = @proposalId;
                    select * from HrProductCountry where ProposalId = @proposalId", 
                new { proposalId });
                var hrProduct = await results.ReadSingleAsync<HrProduct>();
                hrProduct.Countries = await results.ReadAsync<HrProductCountry>();
                return hrProduct;
            }
        }
    }
}
