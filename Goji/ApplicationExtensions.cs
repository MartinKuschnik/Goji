namespace Goji
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using Goji;

    /// <summary>
    /// This class defines extension methods for the <see cref="Application"/> type.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Changes the current UI culture to the given culture.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="value">The new culture.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="application"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="value"/> parameter is <c>null</c>.</exception>
        public static void SetCurrentUICulture(this Application application, CultureInfo value)
        {
            application.VerifyAccess();

            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            CultureInfo oldValue = application.GetCurrentUICulture();

            application.Properties[typeof(CultureInfo)] = value;

            foreach (EventHandler<CultureChangedEventArgs> listener in application.GetListenerCollectionForCurrentUICultureChangedEvent().ToArray().Select(x => x.Key))
            {
                listener(application, new CultureChangedEventArgs(oldValue, value));
            }
        }

        /// <summary>
        /// Gets the current UI culture.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>The current UI culture.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="application"/> parameter is <c>null</c>.</exception>
        public static CultureInfo GetCurrentUICulture(this Application application)
        {
            application.VerifyAccess();

            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            return application.Properties[typeof(CultureInfo)] as CultureInfo ?? CultureInfo.InstalledUICulture;
        }

        /// <summary>
        /// Registers an event handler for the event which occurs when the current UI culture has changed.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="eventhandler">The event handler which should be registered.</param>
        /// <returns>The object to destroy the subscription.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="application"/> parameter is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="eventhandler"/> parameter is <c>null</c>.</exception>
        public static IDisposable RegisterForCurrentUICultureChangedEvent(this Application application, EventHandler<CultureChangedEventArgs> eventhandler)
        {
            application.VerifyAccess();

            if (application == null)
            {
                throw new ArgumentNullException("application");
            }

            if (eventhandler == null)
            {
                throw new ArgumentNullException("eventhandler");
            }

            Subscription<EventHandler<CultureChangedEventArgs>> subscription;

            IDictionary<EventHandler<CultureChangedEventArgs>, Subscription<EventHandler<CultureChangedEventArgs>>> listener = application.GetListenerCollectionForCurrentUICultureChangedEvent();

            if (!listener.TryGetValue(eventhandler, out subscription))
            {
                SubscriptionDisposedHandler subscriptionDisposedHandler = new SubscriptionDisposedHandler(application);

                subscription = new Subscription<EventHandler<CultureChangedEventArgs>>(eventhandler);
                subscription.Disposed += subscriptionDisposedHandler.OnCurrentUICultureChangedEventSubscriptionDestroyed;
                listener.Add(eventhandler, subscription);
            }

            return subscription;
        }

        /// <summary>
        /// Gets the collection of all listeners for the event which occurs when the current UI culture has changed.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>All listeners for the event which occurs when the current UI culture has changed.</returns>
        private static IDictionary<EventHandler<CultureChangedEventArgs>, Subscription<EventHandler<CultureChangedEventArgs>>> GetListenerCollectionForCurrentUICultureChangedEvent(this Application application)
        {
            const string PROPERTY_KEY = "ListenerCollectionForCurrentUICultureChangedEvent";

            IDictionary<EventHandler<CultureChangedEventArgs>, Subscription<EventHandler<CultureChangedEventArgs>>> listeners = application.Properties[PROPERTY_KEY] as IDictionary<EventHandler<CultureChangedEventArgs>, Subscription<EventHandler<CultureChangedEventArgs>>>;

            if (listeners == null)
            {
                listeners = new Dictionary<EventHandler<CultureChangedEventArgs>, Subscription<EventHandler<CultureChangedEventArgs>>>();
                application.Properties[PROPERTY_KEY] = listeners;
            }

            return listeners;
        }

        /// <summary>
        /// Objects of this class handles the Subscription&lt;EventHandler&lt;CultureChangedEventArgs&gt;&gt;.Disposed event.
        /// </summary>
        private class SubscriptionDisposedHandler
        {
            /// <summary>
            /// The referenced application.
            /// </summary>
            private readonly Application application;

            /// <summary>
            /// Initializes a new instance of the <see cref="SubscriptionDisposedHandler"/> class.
            /// </summary>
            /// <param name="application">The referenced application.</param>
            public SubscriptionDisposedHandler(Application application)
            {
                // no parameter validation for internal types
                this.application = application;
            }

            /// <summary>
            /// Called when Subscription&lt;EventHandler&lt;CultureChangedEventArgs&gt;&gt;.Disposed event occurs.
            /// </summary>
            /// <param name="sender">The sender.</param>
            /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
            public void OnCurrentUICultureChangedEventSubscriptionDestroyed(object sender, EventArgs e)
            {
                Subscription<EventHandler<CultureChangedEventArgs>> subscription = sender as Subscription<EventHandler<CultureChangedEventArgs>>;

                if (subscription != null)
                {
                    subscription.Disposed -= this.OnCurrentUICultureChangedEventSubscriptionDestroyed;

                    this.application.GetListenerCollectionForCurrentUICultureChangedEvent().Remove(subscription.EventHandler);
                }
            }
        }
    }
}
