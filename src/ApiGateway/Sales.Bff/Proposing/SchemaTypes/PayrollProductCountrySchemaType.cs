using GraphQL.DataLoader;
using GraphQL.Types;
using Proposing.API.Client;
using Sales.Bff.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public partial class PayrollProductCountrySchemaType : ObjectGraphType<PayrollProductCountryViewModel>
    {
        partial void Extend(ProposingClient client, ReferenceDataCache cache, IDataLoaderContextAccessor accessor)
        {
            Field<PayrollLevelSchemaType>(
                "level",
                resolve: context => cache.Components[context.Source.LevelId]
            );
        }

    }
}
