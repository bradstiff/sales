using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SchemaTypeCodeGenerator
{
    public static class DtoModelLoader
    {
        public static IEnumerable<DtoModel> Load(IEnumerable<Type> dtos, Dictionary<string,string> nameOverrides)
        {
            foreach(var dto in dtos)
            {
                var dtoType = GetDtoType(dto);
                var schemaTypeName = GetSchemaTypeName(dto, nameOverrides);
                var schemaTypeTypeName = GetSchemaTypeTypeName(dto, nameOverrides);
                var apiName = dto.Namespace.Remove(dto.Namespace.IndexOf(".API.Client"));

                yield return new DtoModel
                {
                    Type = dto,
                    DtoType = dtoType,
                    ApiName = apiName,
                    SchemaTypeName = schemaTypeName,
                    SchemaTypeTypeName = schemaTypeTypeName,
                    SchemaTypeNamespace = $"Sales.Bff.{apiName}.SchemaTypes",
                    Properties = dto
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(prop =>
                        {
                            var isList = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>);
                            var type = isList
                                ? prop.PropertyType.GetGenericArguments()[0]
                                : Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                            string graphType = null;
                            var isDto = false;
                            if (new[] { typeof(long), typeof(int), typeof(short), typeof(byte) }.Contains(type))
                            {
                                graphType = "IntGraphType";
                            }
                            else if (new[] { typeof(decimal), typeof(double), typeof(float) }.Contains(type))
                            {
                                graphType = "FloatGraphType";
                            }
                            else if (typeof(bool) == type)
                            {
                                graphType = "BooleanGraphType";
                            }
                            else if (typeof(string) == type)
                            {
                                graphType = "StringGraphType";
                            }
                            else if (dtos.Any(d => d == type))
                            {
                                graphType = GetSchemaTypeTypeName(type, nameOverrides);
                                isDto = true; //view model
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException($"{type.Name} does not have a corresponding GraphType.");
                            }

                            return new DtoPropertyModel
                            {
                                Name = prop.Name,
                                Type = prop.PropertyType,
                                GraphType = graphType,
                                IsList = isList,
                                IsDto = isDto,
                                IsNullable = prop.GetCustomAttribute<RequiredAttribute>() == null
                            };
                        })
                        .ToList()
                };
            }
        }

        private static DtoType GetDtoType(Type dto)
        {
            return dto.Name.EndsWith("ViewModel")
                        ? DtoType.ViewModel
                        : DtoType.Command;
        }
        private static string GetSchemaTypeName(Type dto, Dictionary<string, string> nameOverrides)
        {
            var typeName = dto.Name;
            if (nameOverrides.ContainsKey(typeName))
            {
                return nameOverrides[typeName];
            }
            else if (typeName.StartsWith("ListPageViewModelOf"))
            {
                //e.g., ListPageViewModelOfProposalViewModel => ProposalListPage
                var ofType = typeName.Replace("ListPageViewModelOf", "").Replace("ViewModel", "");
                return ofType + "ListPage";
            }
            return typeName.Remove(typeName.IndexOf(GetDtoType(dto).ToString()));
        }
        private static string GetSchemaTypeTypeName(Type dto, Dictionary<string, string> nameOverrides)
        {
            return GetSchemaTypeName(dto, nameOverrides) + "SchemaType";
        }
    }
}
