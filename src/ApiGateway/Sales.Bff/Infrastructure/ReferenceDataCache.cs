using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Infrastructure
{
    public class ReferenceDataCache
    {
        public ReferenceDataCache(ProposingClient client)
        {
            //todo: need to support async factory method in container
            Load(client);
        }

        public Dictionary<int, ProductModelViewModel> ProductModels { get; private set; }
        private async Task Load(ProposingClient proposingClient)
        {
            var models = await proposingClient.ProductModels_GetAllProductModelsAsync();
            this.ProductModels = models.ToDictionary(m => m.Id);
        }
    }
}
