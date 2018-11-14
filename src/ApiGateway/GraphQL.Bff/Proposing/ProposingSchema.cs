using GraphQL;
using GraphQL.Bff.Proposing;
using GraphQL.Bff.Proposing.SchemaTypes;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class ProposingSchema : Schema
    {
        public ProposingSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<ProposingSchemaQueryRoot>();
        }
    }
}
