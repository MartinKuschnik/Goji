using System;
using System.Diagnostics;

namespace Goji
{
    /// <summary>
    /// Represents a subscription for an event.
    /// </summary>
    internal class Subscription : IDisposable
    {
        /// <summary>
        /// <c>true</c>, if the instance is disposed, otherwise <c>false</c>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isDisposed = false;

        /// <summary>
        /// Occurs when the <see cref="Dispose"/> method was called.
        /// </summary>
        public event EventHandler Disposed;

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
