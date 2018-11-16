using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Schema
{
    public class SalesSchema : GraphQL.Types.Schema
    {
        public SalesSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<SalesSchemaQueryRoot>();
            Mutation = resolver.Resolve<SalesSchemaMutationRoot>();
        }
    }
}
