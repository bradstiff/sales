using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public class HrLevelSchemaType : EnumerationGraphType
    {
        public HrLevelSchemaType()
        {
            Name = "HrLevel";
            AddValue("PA", "Personnel Administration", 13);
            AddValue("ORGMGMT", "Organizational Management", 14);
        }
    }
}
