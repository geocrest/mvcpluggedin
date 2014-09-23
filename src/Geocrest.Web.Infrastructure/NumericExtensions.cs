namespace Geocrest.Web.Infrastructure
{
    using System;

    /// <summary>
    /// Provides helper methods for numeric operations
    /// </summary>
    public static class NumericExtensions
    {
        /// <summary>
        /// Determines if an object is numeric.  Nullable numeric types are considered numeric.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the type can be evaluated as a numeric type; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>Boolean is not considered numeric.</remarks>
        public static bool IsNumericType(this System.Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (System.Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        switch (System.Type.GetTypeCode(Nullable.GetUnderlyingType(type)))
                        {
                            case TypeCode.Byte:
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.SByte:
                            case TypeCode.Single:
                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                                return true;
                        }
                        return false;
                    }
                    return false;
            }
            return false;
        }
    }
}
