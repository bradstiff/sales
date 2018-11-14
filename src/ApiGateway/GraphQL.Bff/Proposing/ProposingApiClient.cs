using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Proposing.API.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GraphQL.Bff.Proposing
{
    public class ProposingApiClient
    {
        private readonly HttpClient _apiClient;
        private readonly ILogger<ProposingApiClient> _logger;
        //private readonly UrlsConfig _urls;

        public ProposingApiClient(HttpClient httpClient, ILogger<ProposingApiClient> logger)//, IOptions<UrlsConfig> config)
        {
            _apiClient = httpClient;
            _logger = logger;
            //_urls = config.Value;
        }

        public async Task<Proposal> GetProposal(int proposalId)
        {
            //var url = _urls.Orders + UrlsConfig.OrdersOperations.GetOrderDraft();
            //var content = new StringContent(JsonConvert.SerializeObject(basket), System.Text.Encoding.UTF8, "application/json");
            var url = $"http://localhost:10598/api/proposals/{proposalId}";
            var response = await _apiClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Proposal>(responseContent);
        }

        public async Task<List<Proposal>> GetProposals()
        {
            var url = $"localhost:10598/proposals";
            var response = await _apiClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Proposal>>(responseContent);
        }
    }
}
