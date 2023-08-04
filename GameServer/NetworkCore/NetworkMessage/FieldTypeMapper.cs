using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public static class FieldTypeMapper
    {
        private static readonly Dictionary<FieldType, Type> fieldTypeMap = new Dictionary<FieldType, Type>
        {
            { FieldType.field_int, typeof(int) },
            { FieldType.field_short, typeof(short) },
            { FieldType.field_long, typeof(long) },
            { FieldType.field_double, typeof(double) },
            { FieldType.field_float, typeof(float) },
            { FieldType.field_string, typeof(string) },
        };

        private static readonly Dictionary<Type, FieldType> reverseFieldTypeMap = fieldTypeMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public static Type GetFieldType(FieldType fieldType)
        {
            if (fieldTypeMap.TryGetValue(fieldType, out Type type))
            {
                return type;
            }

            throw new NotSupportedException("FieldType not supported.");
        }

        public static FieldType GetFieldType(Type type)
        {
            if (reverseFieldTypeMap.TryGetValue(type, out FieldType fieldType))
            {
                return fieldType;
            }

            throw new NotSupportedException("FieldType not supported.");
        }
    }
}
