using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SchemaTypeCodeGenerator
{
    public class CodeGenerator
    {
        public void Generate()
        {
            const string basePath = @"C:\Users\Brad\Source\Repos\sales\src\ApiGateway\Sales.Bff";

            var overrides = new Dictionary<string, string>
            {
                { "ProposalCountryDto", "ProposalCountryInput" },
                { "UpdatePayrollCountryScopeDto", "UpdatePayrollCountryScope" },
            };
            var assemblyPath = @"C:\Users\Brad\Source\Repos\sales\src\ApiGateway\Sales.Bff\bin\Debug\netcoreapp2.2\Sales.Bff.dll";
            var dtos = AssemblyScanner.Scan(assemblyPath, t => t.Namespace.EndsWith("API.Client") && (t.Name.EndsWith("ViewModel") || t.Name.EndsWith("Command")), overrides.Keys);

            var dtoModels = DtoModelLoader.Load(dtos, overrides);
            foreach (var dtoModel in dtoModels)
            {
                Console.WriteLine($"Generating {dtoModel.SchemaTypeTypeName}");
                var schemaType = new SchemaType(dtoModel).TransformText();
                File.WriteAllText(Path.Combine(basePath, dtoModel.ApiName, "GeneratedSchemaTypes", dtoModel.SchemaTypeTypeName + ".cs"), schemaType);
            }
        }
    }
}