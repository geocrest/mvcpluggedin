namespace Geocrest.Web.Infrastructure
{
    using System;
    using System.Configuration;
    using System.Configuration.Provider;
    using System.Diagnostics;

    /// <summary>
    /// Throws handled exceptions.
    /// </summary>
    public static partial class Throw
    {
        #region Methods
        /// <summary>
        /// Throws the specified exception.
        /// </summary>
        /// <param name="exception">The exception to be thrown.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        private static void ThrowInternal(Exception exception, Func<Exception, Exception> modifier = null)
        {
            if (exception == null)
                return;

            Exception ex = null;
            if (modifier != null)
                ex = modifier(exception);

            /* We should never try to suppress an exception at this point, so make sure the original
             * exception is thrown if the modifier function returns null. */
            if (ex == null)
                ex = exception;
            
            throw ex;
        }
       
        /// <summary>
        /// Throws a <see cref="T:System.NotSupportedException" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void NotSupported(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new NotSupportedException(message), modifier);
        }

        /// <summary>
        /// Throws a <see cref="T:System.NotSupportedException" /> with the specified message.
        /// </summary>
        /// <typeparam name="T">The type of argument under test.</typeparam>
        /// <param name="argument">The argument to test.</param>
        /// <param name="clause">The function used to evaluate the argument.</param>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
       [DebuggerStepThrough]
        public static void If<T>(T argument, Func<T, bool> clause, string message, Func<Exception, Exception> modifier = null)
        {
            if (clause(argument) == true)
                ThrowInternal(new NotSupportedException(message), modifier);
        }
       
        /// <summary>
       /// Throws an <see cref="T:System.InvalidOperationException" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void InvalidOperation(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new InvalidOperationException(message), modifier);
        }

        /// <summary>
        /// Throws a <see cref="T:System.NotImplementedException" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void NotImplemented(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new NotImplementedException(message), modifier);
        }

        /// <summary>
        /// Throws a <see cref="T:System.Configuration.ConfigurationErrorsException" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void ConfigurationErrors(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new ConfigurationErrorsException(message), modifier);
        }

        /// <summary>
        /// Throws a <see cref="T:System.Configuration.SettingsPropertyNotFoundException" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void SettingsPropertyNotFound(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new SettingsPropertyNotFoundException(message), modifier);
        }

        /// <summary>
        /// Throws a generic <see cref="T:System.Exception" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void Generic(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new Exception(message), modifier);
        }

        /// <summary>
        /// Throws a generic <see cref="T:System.Exception" /> using the specified exception.
        /// </summary>
        /// <param name="exception">The exception to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void Generic(Exception exception, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(exception, modifier);
        }

        /// <summary>
        /// Throws a <see cref="T:System.Configuration.Provider.ProviderException" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void ProviderException(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new ProviderException(message), modifier);
        }
        #endregion
    }
}