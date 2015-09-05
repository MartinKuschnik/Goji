namespace Goji
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Provides the event which occurs when a localization property has changed.
    /// </summary>
    internal class WeakDependencyPropertyEventBus
    {
        /// <summary>
        /// Contains all subscriptions.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Subscription<PropertyChangedCallback>, WeakReference> subscriptions = new Dictionary<Subscription<PropertyChangedCallback>, WeakReference>();

        /// <summary>
        /// Register a <see cref="PropertyChangedCallback"/> for the event which occurs when a localization property has changed.
        /// </summary>
        /// <param name="eventHandler">The event handler which should be called if the a localization property has changed.</param>
        /// <returns>The object to destroy the subscription.</returns>
        public IDisposable CreateSubscription(PropertyChangedCallback eventHandler)
        {
            Subscription<PropertyChangedCallback> subscription = new Subscription<PropertyChangedCallback>(eventHandler);

            subscription.Disposed += this.OnSubscriptionDisposed;

            this.subscriptions.Add(subscription, new WeakReference(eventHandler));

            return subscription;
        }

        /// <summary>
        /// Notifies all subscribers that a localization property has changed.
        /// </summary>
        /// <param name="dp">The localization property which has changes.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public void NotifySubscribers(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            foreach (var subscriptionKV in this.subscriptions.ToArray())
            {
                PropertyChangedCallback target = subscriptionKV.Value.Target as PropertyChangedCallback;

                if (target != null)
                {
                    target.Invoke(dp, args);
                }
                else
                {
                    // dispose the subscription if the object is already collected from the GC
                    subscriptionKV.Key.Dispose();
                }
            }
        }

        /// <summary>
        /// Called when subscription  was disposed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSubscriptionDisposed(object sender, EventArgs e)
        {
            Subscription<PropertyChangedCallback> subscription = sender as Subscription<PropertyChangedCallback>;

            if (sender != null)
            {
                subscription.Disposed -= this.OnSubscriptionDisposed;

                this.subscriptions.Remove(subscription);
            }
        }
    }
}
