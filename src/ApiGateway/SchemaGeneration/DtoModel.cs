﻿using System;
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
        public bool IsId { get; set; }
        public Type ListGenericArgument { get; set; }
        public bool IsList => this.ListGenericArgument != null;
        public string ListGraphTypeArgument { get; set; }
        public bool IsNullable { get; set; }
    }
}