using GraphQL.Types;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public class EnumeratedComponentSchemaType<TEnum> : ObjectGraphType<ComponentViewModel>
        where TEnum: EnumerationGraphType
    {
        public EnumeratedComponentSchemaType()
        {
            var displayAttribute = (DisplayAttribute)this.GetType().GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            Name = displayAttribute?.Name ?? "EnumeratedComponent";

            Field<TEnum>(
                name: "Id",
                resolve: context => context.Source.Id
            );
            Field(x => x.Name, nullable: true);
            Field(x => x.FullName, nullable: true);
            Field(x => x.IsActive, nullable: true);
            Field(x => x.SortOrder, nullable: true);
        }
    }
}
