namespace Goji
{
    using System;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// <see cref="EventArgs"/> to transport the old <see cref="CultureInfo"/> object an a new one.
    /// </summary>
    public class CultureChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The old culture.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly CultureInfo oldValue;

        /// <summary>
        /// The new culture.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly CultureInfo newValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldValue">The old culture.</param>
        /// <param name="newValue">The new culture.</param>
        public CultureChangedEventArgs(CultureInfo oldValue, CultureInfo newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        /// <summary>
        /// Gets the old culture.
        /// </summary>
        /// <value>
        /// The old culture.
        /// </value>
        public CultureInfo OldValue
        {
            get { return this.oldValue; }
        }

        /// <summary>
        /// Gets the new culture.
        /// </summary>
        /// <value>
        /// The new culture.
        /// </value>
        public CultureInfo NewValue
        {
            get { return this.newValue; }
        }
    }
}
