using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Infrastructure
{
    public class ReferenceDataCache
    {
        private List<ProductDefinitionViewModel> _products;
        private Dictionary<int, ComponentViewModel> _components;
        private Dictionary<int, List<ComponentViewModel>> _componentsByType;

        public ReferenceDataCache(ProposingClient client)
        {
            //todo: need to support async factory method in container
            Load(client);
        }

        public List<ProductDefinitionViewModel> Products => _products; 
        public Dictionary<int, ComponentViewModel> Components => _components;
        public Dictionary<int, List<ComponentViewModel>> ComponentsByType => _componentsByType;

        private async Task Load(ProposingClient proposingClient)
        {
            var productsTask = proposingClient.Products_GetProductsAsync();
            var componentsTask = proposingClient.Products_GetComponentsAsync();
            await Task.WhenAll(productsTask, componentsTask);

            _products = productsTask.Result.ToList();
            _components = componentsTask.Result.ToDictionary(c => c.Id);
            _componentsByType = componentsTask.Result
                .Where(c => c.ComponentTypeID.HasValue)
                .GroupBy(c => c.ComponentTypeID.Value)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }
}
