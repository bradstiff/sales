using GraphQL.Types;
using Proposing.API.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public class PayrollProductModelSchemaType : ObjectGraphType
    {
        public PayrollProductModelSchemaType()
        {
            Name = "PayrollProductModel";
            Field<ListGraphType<PayrollLevelSchemaType>>("Levels");
        }

        public static object Resolve(IEnumerable<ComponentViewModel> components)
        {
            return new
            {
                Levels = components.Where(c => c.ComponentTypeID == 2)
            };
        }
    }

    public class PayrollLevelEnumSchemaType : EnumerationGraphType
    {
        //todo: hard-coded
        public PayrollLevelEnumSchemaType()
        {
            Name = "PayrollLevelEnum";
            AddValue("PS", "Processing Service", 4);
            AddValue("MS", "Managed Service", 5);
            AddValue("COS", "Comprehensive Service", 6);
        }
    }

    [Display(Name = "PayrollLevel")]
    public class PayrollLevelSchemaType : EnumeratedComponentSchemaType<PayrollLevelEnumSchemaType> { }
}
