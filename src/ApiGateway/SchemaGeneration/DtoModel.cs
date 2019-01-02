using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemaTypeCodeGenerator
{
    public class DtoModel
    {
        public Type Type{ get; set; }
        public DtoType DtoType { get; set; }
        public string ApiName { get; set; }
        public string SchemaTypeName { get; set; }
        public string SchemaTypeBaseName => $"{(this.DtoType == DtoType.ViewModel ? "ObjectGraphType" : "InputObjectGraphType")}<{this.Type.Name}>";
        public string SchemaTypeTypeName { get; set; }
        public string SchemaTypeNamespace { get; set; }
        public List<DtoPropertyModel> Properties { get; set; }
    }

    public class DtoPropertyModel
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string GraphType { get; set; }
        public bool IsId => this.Name == "Id";
        public bool IsList { get; set; }
        public bool IsDto { get; set; }
        public bool IsNullable { get; set; }
    }
}
