namespace Goji
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents a subscription of one event handler for an event.
    /// </summary>
    /// <typeparam name="TEventHandler">The type of the event handler.</typeparam>
    internal class Subscription<TEventHandler> : IDisposable
        where TEventHandler : class
    {
        /// <summary>
        /// The event handler.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TEventHandler eventhandler;

        /// <summary>
        /// <c>true</c>, if the instance is disposed, otherwise <c>false</c>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isDisposed = false;

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
        /// Occurs when the <see cref="Dispose"/> method was called.
        /// </summary>
        public event EventHandler Disposed;

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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.isDisposed = true;

                EventHandler disposedEvent = this.Disposed;

                if (disposedEvent != null)
                {
                    disposedEvent(this, EventArgs.Empty);
                }
            }
        }
    }
}
