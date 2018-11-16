using System;
using System.Collections.Generic;
using System.Text;

namespace SchemaTypeCodeGenerator
{
    public partial class SchemaType
    {
        private Message _message;
        public SchemaType(Message message)
        {
            _message = message;
        }
    }
}
