
namespace Geocrest.Web.Infrastructure
{
    using System;

    /// <summary>
    /// Provides an indicator as to whether an object's value has been locked.
    /// </summary>
    /// <typeparam name="TValue">The type of object to be locked.</typeparam>
    public struct LockValue<TValue>
    {
        private bool _isLocked;
        private TValue _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Geocrest.Web.Infrastructure.LockValue`1" /> struct.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public LockValue(TValue initialValue)
        {
            this._value = initialValue;
            this._isLocked = false;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public TValue Value
        {
            get { return this._value; }
            set
            {
                if (this._isLocked)
                {
                    Throw.InvalidOperation("Property cannot be set: already locked");
                }

                this._value = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is locked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is locked; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocked
        {
            get { return this._isLocked; }
        }

        /// <summary>
        /// Locks this instance.
        /// </summary>
        public void Lock()
        {
            this._isLocked = true;
        }

        /// <summary>
        /// Specifies the type of value to be locked.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:`0"/>.
        /// </returns>
        public static implicit operator TValue(LockValue<TValue> obj)
        {
            return obj.Value;
        }
        /// <summary>
        /// Creates a new <see cref="T:Geocrest.Web.Infrastructure.LockValue`1" />
        /// with the specified <typeparamref name="TValue" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Returns an instance of <see cref="T:Geocrest.Web.Infrastructure.LockValue``0" />.
        /// </returns>
        public static implicit operator LockValue<TValue>(TValue value)
        {
            return new LockValue<TValue>(value);
        }
    }
}
