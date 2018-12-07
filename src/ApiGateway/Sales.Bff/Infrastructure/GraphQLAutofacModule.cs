using Autofac;
using GraphQL.Types;
using Sales.Bff.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sales.Bff.Infrastructure
{
    public class GraphQLAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(SalesSchema).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ObjectGraphType<>));

            builder
                .RegisterAssemblyTypes(typeof(SalesSchema).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(InputObjectGraphType<>));

            builder
                .RegisterAssemblyTypes(typeof(SalesSchema).GetTypeInfo().Assembly)
                .AssignableTo(typeof(EnumerationGraphType));
        }
    }
}
