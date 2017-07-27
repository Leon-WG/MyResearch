using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Common.Extensions
{
    public static class TypeExtensions
    {
        public static string GetFullNameWithoutNamespace(this Type type)
        {
            if (type.IsGenericParameter)
                return type.Name;
            if (string.IsNullOrEmpty(type.Namespace))
                return type.FullName;
            return type.FullName.Substring(type.Namespace.Length + 1);
        }

        public static Type[] GetAllGenericArguments(this TypeInfo type)
        {
            if (type.GenericTypeArguments.Length == 0)
                return type.GenericTypeParameters;
            return type.GenericTypeArguments;
        }

        public static string ToGenericTypeString(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType)
                return type.GetFullNameWithoutNamespace().ReplacePlusWithDotInNestedTypeName();
            return type.GetGenericTypeDefinition().GetFullNameWithoutNamespace().ReplacePlusWithDotInNestedTypeName().ReplaceGenericParametersInGenericTypeName(type);
        }

        private static string ReplacePlusWithDotInNestedTypeName(this string typeName)
        {
            return typeName.Replace('+', '.');
        }

        private static string ReplaceGenericParametersInGenericTypeName(this string typeName, Type type)
        {
            Type[] genericArguments = type.GetTypeInfo().GetAllGenericArguments();
            typeName = new Regex("`[1-9]\\d*").Replace(typeName, (MatchEvaluator)(match =>
            {
                int count = int.Parse(match.Value.Substring(1));
                string str = string.Join(",", ((IEnumerable<Type>)genericArguments).Take<Type>(count).Select<Type, string>(new Func<Type, string>(TypeExtensions.ToGenericTypeString)));
                genericArguments = ((IEnumerable<Type>)genericArguments).Skip<Type>(count).ToArray<Type>();
                return "<" + str + ">";
            }));
            return typeName;
        }
    }
}
