using System;
using System.Numerics;

namespace Experiment.InternalClassLibrary
{
    public static class GenericMethods
    {
        public static VALUE_T Minimum1<VALUE_T>(VALUE_T x, VALUE_T y)
            where VALUE_T : IComparable<VALUE_T>
            => x is null
                ? y
                : y is null
                ? x
                : x.CompareTo(y) > 0
                ? y
                : x;

        public static VALUE_T Minimum2<VALUE_T>(VALUE_T x, VALUE_T y)
            where VALUE_T : IComparisonOperators<VALUE_T, VALUE_T, bool>
            => x is null
                ? y
                : y is null
                ? x
                : x > y
                ? y
                : x;

        public static int GetSizeOfType1<TYPE_T>()
        {
            if (typeof(TYPE_T) == typeof(bool))
                return sizeof(bool);
            else if (typeof(TYPE_T) == typeof(char))
                return sizeof(char);
            else if (typeof(TYPE_T) == typeof(byte))
                return sizeof(byte);
            else if (typeof(TYPE_T) == typeof(sbyte))
                return sizeof(sbyte);
            else if (typeof(TYPE_T) == typeof(short))
                return sizeof(short);
            else if (typeof(TYPE_T) == typeof(ushort))
                return sizeof(ushort);
            else if (typeof(TYPE_T) == typeof(int))
                return sizeof(int);
            else if (typeof(TYPE_T) == typeof(uint))
                return sizeof(uint);
            else if (typeof(TYPE_T) == typeof(long))
                return sizeof(ulong);
            else if (typeof(TYPE_T) == typeof(float))
                return sizeof(float);
            else if (typeof(TYPE_T) == typeof(double))
                return sizeof(double);
            else if (typeof(TYPE_T) == typeof(decimal))
                return sizeof(decimal);
            else
                throw new Exception();
        }

        public static int GetSizeOfType2<TYPE_T>()
            => Type.GetTypeCode(typeof(TYPE_T)) switch
            {
                TypeCode.Boolean => sizeof(bool),
                TypeCode.Char => sizeof(char),
                TypeCode.SByte => sizeof(sbyte),
                TypeCode.Byte => sizeof(byte),
                TypeCode.Int16 => sizeof(short),
                TypeCode.UInt16 => sizeof(ushort),
                TypeCode.Int32 => sizeof(int),
                TypeCode.UInt32 => sizeof(uint),
                TypeCode.Int64 => sizeof(long),
                TypeCode.UInt64 => sizeof(ulong),
                TypeCode.Single => sizeof(float),
                TypeCode.Double => sizeof(double),
                TypeCode.Decimal => sizeof(decimal),
                _ => throw new Exception(),
            };
    }
}
