using GraphQL.Types;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public class HrProductModelSchemaType : ObjectGraphType
    {
        public HrProductModelSchemaType()
        {
            Name = "HrProductModel";
            Field<ListGraphType<HrLevelSchemaType>>("Levels");
        }

        public static object Resolve(IEnumerable<ComponentViewModel> components)
        {
            return new
            {
                Levels = components.Where(c => c.ComponentTypeID == 10)
            };
        }
    }

    public class HrLevelEnumSchemaType : EnumerationGraphType
    {
        public HrLevelEnumSchemaType()
        {
            Name = "HrLevelEnum";
            AddValue("PA", "Personnel Administration", 13);
            AddValue("OM", "Organizational Management", 14);
        }
    }

    [Display(Name = "HrLevel")]
    public class HrLevelSchemaType : EnumeratedComponentSchemaType<HrLevelEnumSchemaType> { }
}
