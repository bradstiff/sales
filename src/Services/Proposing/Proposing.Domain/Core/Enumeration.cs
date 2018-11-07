using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Proposing.Domain.Core
{
    public abstract class Enumeration<T> where T : struct 
    {
        public string DisplayName { get; private set; }

        public T Value { get; private set; }

        protected Enumeration()
        { }

        protected Enumeration(T value, string name)
        {
            Value = value;
            DisplayName = name;
        }

        public override string ToString() => DisplayName;

        public static IEnumerable<TEnum> GetAll<TEnum>() where TEnum : Enumeration<T>
        {
            var fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<TEnum>();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Enumeration<T> otherValue))
                return false;

            if (!GetType().Equals(obj.GetType()))
                return false;

            if (obj == null)
                return false;

            return this.Value.Equals(otherValue.Value);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static TEnum FromValue<TEnum>(T value) where TEnum : Enumeration<T>
        {
            var matchingItem = Parse<TEnum, T>(value, "value", item => item.Value.Equals(value));
            return matchingItem;
        }

        public static TEnum FromDisplayName<TEnum>(string displayName) where TEnum : Enumeration<T>
        {
            var matchingItem = Parse<TEnum, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        private static TEnum Parse<TEnum, K>(K value, string description, Func<TEnum, bool> predicate) where TEnum : Enumeration<T>
        {
            var matchingItem = GetAll<TEnum>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(TEnum)}");

            return matchingItem;
        }

        public static bool IsFlagSet(long value, Enumeration<long> flag)
        {
            return (value & flag.Value) == flag.Value;
        }

        public static bool IsFlagSet(int value, Enumeration<int> flag)
        {
            return (value & flag.Value) == flag.Value;
        }

        public static bool IsFlagSet(short value, Enumeration<short> flag)
        {
            return (value & flag.Value) == flag.Value;
        }

        public static bool IsFlagSet(byte value, Enumeration<byte> flag)
        {
            return (value & flag.Value) == flag.Value;
        }
    }
}
