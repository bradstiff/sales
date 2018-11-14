using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Bff.Proposing.SchemaTypes
{
    public class CreateProposalInputType : InputObjectGraphType
    {
        public CreateProposalInputType()
        {
            Name = "CreateProposalInput";
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<StringGraphType>>("clientName");
            Field<StringGraphType>("comments");
            Field<NonNullGraphType<ListGraphType<IntGraphType>>>("countryIds");
        }
    }
}
