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
        public static List<Type> Scan(string assemblyPath, Func<Type, bool> predicate)
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

            var dtos = new List<Type>();
            types.ForEach(type => TryAddType(type, dtos, predicate));
            return dtos;
        }

        public static void TryAddType(Type type, List<Type> types, Func<Type, bool> predicate)
        {
            try
            {
                if (predicate(type))
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
