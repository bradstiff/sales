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
        private MessageType GetMessageType(Type message)
        {
            return message.Name.EndsWith("ViewModel")
                        ? MessageType.ViewModel
                        : MessageType.Command;
        }
        private string GetSchemaTypeName(Type message)
        {
            var typeName = message.Name;
            if (typeName.StartsWith("ListPageViewModelOf"))
            {
                //e.g., ListPageViewModelOfProposalViewModel => ProposalListPage
                var ofType = typeName.Replace("ListPageViewModelOf", "").Replace("ViewModel", "");
                return ofType + "ListPage";
            }
            return typeName.Remove(typeName.IndexOf(this.GetMessageType(message).ToString()));
        }
        private string GetSchemaTypeTypeName(Type message)
        {
            return this.GetSchemaTypeName(message) + "Type";
        }
        public void Generate()
        {
            var basePath = @"C:\Users\Brad\Source\Repos\sales\src\ApiGateway\Sales.Bff";
            var assembly = Assembly.LoadFrom(@"C:\Users\Brad\Source\Repos\sales\src\ApiGateway\Sales.Bff\bin\Release\netcoreapp2.1\publish\Sales.Bff.dll");
            List<Type> messageTypes;
            try
            {
                messageTypes = assembly.GetTypes().ToList();
            }
            catch (ReflectionTypeLoadException e)
            {
                messageTypes = e.Types.Where(t => t != null).ToList();
            }
            messageTypes
                .Where(t => t.Namespace.EndsWith("API.Client") && (t.Name.EndsWith("ViewModel") || t.Name.EndsWith("Command")))
                .Select(message =>
                {
                    var messageType = this.GetMessageType(message);
                    var schemaTypeName = this.GetSchemaTypeName(message);
                    var schemaTypeTypeName = this.GetSchemaTypeTypeName(message);
                    var apiName = message.Namespace.Remove(message.Namespace.IndexOf(".API.Client"));
                    return new Message
                    {
                        Type = message,
                        MessageType = messageType,
                        ApiName = apiName,
                        SchemaTypeName = schemaTypeName,
                        SchemaTypeTypeName = schemaTypeTypeName,
                        SchemaTypeNamespace = $"Sales.Bff.{apiName}.SchemaTypes",
                        Properties = message
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
                                    else if (messageTypes.Any(type => type == listGenericArgument))
                                    {
                                        listGraphTypeArgument = this.GetSchemaTypeTypeName(listGenericArgument);
                                    }
                                    else
                                    {
                                        throw new ArgumentOutOfRangeException($"{listGenericArgument.Name} does not have a corresponding GraphType.");
                                    }
                                }

                                return new MessageProperty
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
                })
                .ToList().ForEach(message =>
                {
                    var generator = new SchemaType(message);
                    var schemaType = generator.TransformText();
                    Console.WriteLine($"Generating {message.SchemaTypeTypeName}");
                    File.WriteAllText(Path.Combine(basePath, message.ApiName, "GeneratedSchemaTypes", message.SchemaTypeTypeName + ".cs"), schemaType);
                });
        }
    }

    public static class AssignableExtensions
    {
        /// <summary>
        /// Determines whether the <paramref name="genericType"/> is assignable from
        /// <paramref name="givenType"/> taking into account generic definitions
        /// </summary>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType == null || genericType == null)
            {
                return false;
            }

            return givenType == genericType
                || givenType.MapsToGenericTypeDefinition(genericType)
                || givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
                || givenType.BaseType.IsAssignableToGenericType(genericType);
        }

        private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return givenType
                .GetInterfaces()
                .Where(it => it.IsGenericType)
                .Any(it => it.GetGenericTypeDefinition() == genericType);
        }

        private static bool MapsToGenericTypeDefinition(this Type givenType, Type genericType)
        {
            return genericType.IsGenericTypeDefinition
                && givenType.IsGenericType
                && givenType.GetGenericTypeDefinition() == genericType;
        }
    }
}