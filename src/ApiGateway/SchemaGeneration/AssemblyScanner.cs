using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SchemaTypeCodeGenerator
{
    public static class AssemblyScanner
    {
        public static List<Type> Scan(string assemblyPath, Func<Type, bool> predicate, IEnumerable<string> typeNamesToForceInclude)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            List<Type> types;
            try
            {
                types = assembly.GetTypes().ToList();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types.Where(t => t != null).ToList();
            }

            bool predicateWithOverrides(Type type) => predicate(type) || typeNamesToForceInclude.Any(o => o == type.Name);
            var dtos = new List<Type>();
            types.ForEach(type => TryAddType(type, dtos, predicateWithOverrides));
            return dtos;
        }

        public static void TryAddType(Type type, List<Type> types, Func<Type, bool> predicate)
        {
            try
            {
                //Anonymous types have Namespace == null
                if (type.Namespace != null && predicate(type))
                {
                    types.Add(type);
                }
            }
            catch (FileNotFoundException)
            {
                //we don't need to scan dependencies
            }
        }
    }
}
