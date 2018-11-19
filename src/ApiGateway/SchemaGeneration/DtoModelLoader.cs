﻿using System;
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
        public static IEnumerable<DtoModel> Load(IEnumerable<Type> dtos)
        {
            foreach(var dto in dtos)
            {
                var dtoType = GetDtoType(dto);
                var schemaTypeName = GetSchemaTypeName(dto);
                var schemaTypeTypeName = GetSchemaTypeTypeName(dto);
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
                            Type listGenericArgument = null;
                            string listGraphTypeArgument = null;
                            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                            {
                                listGenericArgument = prop.PropertyType.GetGenericArguments()[0];
                                if (new[] { typeof(long), typeof(int), typeof(short), typeof(byte) }.Contains(listGenericArgument))
                                {
                                    listGraphTypeArgument = "IntGraphType";
                                }
                                else if (new[] { typeof(decimal), typeof(double), typeof(float) }.Contains(listGenericArgument))
                                {
                                    listGraphTypeArgument = "FloatGraphType";
                                }
                                else if (typeof(bool) == listGenericArgument)
                                {
                                    listGraphTypeArgument = "BooleanGraphType";
                                }
                                else if (typeof(string) == listGenericArgument)
                                {
                                    listGraphTypeArgument = "StringGraphType";
                                }
                                else if (dtos.Any(type => type == listGenericArgument))
                                {
                                    listGraphTypeArgument = GetSchemaTypeTypeName(listGenericArgument);
                                }
                                else
                                {
                                    throw new ArgumentOutOfRangeException($"{listGenericArgument.Name} does not have a corresponding GraphType.");
                                }
                            }

                            return new DtoPropertyModel
                            {
                                Name = prop.Name,
                                Type = prop.PropertyType,
                                IsId = prop.Name == "Id",
                                ListGenericArgument = listGenericArgument,
                                ListGraphTypeArgument = listGraphTypeArgument,
                                IsNullable = prop.GetCustomAttribute<RequiredAttribute>() == null
                            };
                        })
                        .ToList()
                };
            }
        }

        private static DtoType GetDtoType(Type message)
        {
            return message.Name.EndsWith("ViewModel")
                        ? DtoType.ViewModel
                        : DtoType.Command;
        }
        private static string GetSchemaTypeName(Type message)
        {
            var typeName = message.Name;
            if (typeName.StartsWith("ListPageViewModelOf"))
            {
                //e.g., ListPageViewModelOfProposalViewModel => ProposalListPage
                var ofType = typeName.Replace("ListPageViewModelOf", "").Replace("ViewModel", "");
                return ofType + "ListPage";
            }
            return typeName.Remove(typeName.IndexOf(GetDtoType(message).ToString()));
        }
        private static string GetSchemaTypeTypeName(Type message)
        {
            return GetSchemaTypeName(message) + "Type";
        }
    }
}