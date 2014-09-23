namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;

    public static partial class Throw
    {
        #region Methods
        /// <summary>
        /// Throws an <see cref="T:System.ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNull(object argument, string argumentName, Func<Exception, Exception> modifier = null)
        {
            if (argument == null)
                ThrowInternal(
                    new ArgumentNullException(argumentName),
                    modifier);
        }
        /// <summary>
        /// Throws an <see cref="T:System.ArgumentException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void ArgumentException(string argumentName, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(
                new ArgumentException(argumentName),
                modifier);
        }
        /// <summary>
        /// Throws an <see cref="T:System.ArgumentNullException" /> if the specified argument is null or equal to <see cref="String.Empty" />.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNullOrEmpty(string argument, string argumentName, Func<Exception, Exception> modifier = null)
        {
            if (string.IsNullOrEmpty(argument))
                ThrowInternal(
                    new ArgumentNullException(
                        string.Format(CultureInfo.CurrentUICulture, Resources.Exceptions.ArgumentNullOrEmpty, argumentName)),
                    modifier);
        }
        /// <summary>
        /// Throws an <see cref="T:System.ArgumentNullException" /> if the specified argument is null or equal to <see cref="String.Empty" />.
        /// </summary>
        /// <typeparam name="T">The type argument of the <see cref="T:System.Nullable`1"/>.</typeparam>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfNullableIsNull<T>(Nullable<T> argument, string argumentName, Func<Exception, Exception> modifier = null) where T : struct,IComparable
        {
            if (!argument.HasValue)
                ThrowInternal(
                    new ArgumentNullException(
                        string.Format(CultureInfo.CurrentUICulture, Resources.Exceptions.ArgumentNullOrEmpty, argumentName)),
                    modifier);
        }
        /// <summary>
        /// Throws an <see cref="T:System.ArgumentOutOfRangeException" /> exception.
        /// </summary>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void ArgumentOutOfRange(string argumentName, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(
                new ArgumentOutOfRangeException(
                    string.Format(CultureInfo.CurrentUICulture, Resources.Exceptions.ArgumentOutOfRange,
                    argumentName)),
                modifier);
        }
        /// <summary>
        /// Throws a <see cref="T:System.ComponentModel.InvalidEnumArgumentException" /> exception if the
        /// argument is not defined or a <see cref="T:System.NotSupportedException"/> exception if 
        /// <typeparamref name="TEnum"/> is not an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of enum.</typeparam>
        /// <param name="value">The value to test.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfEnumNotDefined<TEnum>(object value, string argumentName, Func<Exception, Exception> modifier = null)
        {
            if (!typeof(TEnum).IsEnum)
            {
                ThrowInternal(new NotSupportedException(
                                    string.Format(CultureInfo.CurrentUICulture, Resources.Exceptions.EnumNotDefined,
                                    typeof(TEnum).FullName)));
                return;
            }
            if (!Enum.IsDefined(typeof(TEnum),value))
                ThrowInternal(new InvalidEnumArgumentException(argumentName,(int)(value), typeof(TEnum)));

        }
        #endregion
    }
}