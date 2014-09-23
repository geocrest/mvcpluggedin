namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    /// <summary> 
    /// Provides helper methods for enumerations 
    /// </summary> 
    public static class EnumerationExtensions
    {
        /// <summary>Gets an enum value's description.</summary> 
        /// <param name="value">The value.</param> 
        /// <returns> 
        /// The description attribute associated with the <c>enum</c> value. 
        /// </returns> 
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null) return string.Empty;
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        /// <summary>
        /// Gets the values of an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type.</typeparam>
        /// <returns>
        /// Returns an instance of <see cref="T:System.Collections.Generic.IDictionary`2" />
        /// </returns>
        /// <exception cref="System.ArgumentException"><typeparamref name="TEnum"/> is not an enum</exception>
        public static IDictionary<string, TEnum> GetEnumValues<TEnum>()
        {
            Type enumType = typeof(TEnum);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }
            IDictionary<string, TEnum> values = new Dictionary<string, TEnum>();
            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                string desc = "";
                DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

                object value = field.GetValue(enumType);
                if (attributes != null &&
                    attributes.Length > 0)
                    desc = attributes[0].Description;
                else
                    desc = value.ToString();
                values.Add(desc, (TEnum)value);
            }
            return values;
        }
    }
}