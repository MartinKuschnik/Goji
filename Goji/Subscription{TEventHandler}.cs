namespace Goji
{
    using System.Diagnostics;

    /// <summary>
    /// Represents a subscription of one event handler for an event.
    /// </summary>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    internal class Subscription<TEventHandler> : Subscription
        where TEventHandler : class
    {
        /// <summary>
        /// The event handler.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TEventHandler eventhandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscription{TEventHandler}"/> class.
        /// </summary>
        /// <param name="eventhandler">The event handler.</param>
        internal Subscription(TEventHandler eventhandler)
        {
            // internal class -> no parameter validation
            this.eventhandler = eventhandler;
        }

        /// <summary>
        /// Gets the event handler.
        /// </summary>
        /// <value>
        /// The event handler.
        /// </value>
        public TEventHandler EventHandler
        {
            get
            {
                return this.eventhandler;
            }
        }
    }
}
