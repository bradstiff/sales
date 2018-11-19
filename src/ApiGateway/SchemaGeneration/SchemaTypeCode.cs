using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaTypeCodeGenerator
{
    public partial class SchemaType
    {
        private DtoModel _message;
        public SchemaType(DtoModel message)
        {
            _message = message;
        }
    }
}
