namespace Goji.Data
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Defines an object which can be used to bind a <see cref="DependencyProperty"/> of an <see cref="DependencyObject"/> over a <see cref="System.Windows.Data.Binding"/> to the UI.
    /// </summary>
    internal class LocalizationPropertyBindingSource : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Defines the name of the <see cref="Value"/> property.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const string BoundPropertyName = "Value";

        /// <summary>
        /// The target <see cref="DependencyObject"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DependencyProperty targetProperty;

        /// <summary>
        /// The subscription for the event which occurs when one localization property has changed.
        /// </summary>
        private readonly IDisposable localizationPropertyChangedEventsSubscription;

        /// <summary>
        /// The target <see cref="DependencyProperty"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DependencyObject targetObject;

        /// <summary>
        /// The object which provides the event which occurs when one localization property has changed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly WeakDependencyPropertyEventBus localizationPropertyChangedEvents;

        /// <summary>
        /// The Lazy&lt;Binding&gt; object which created the <see cref="System.Windows.Data.Binding"/> if it's needed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Binding> bindng;

        /// <summary>
        /// <c>true</c>, if the instance is already disposed, otherwise <c>false</c>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationPropertyBindingSource"/> class.
        /// </summary>
        /// <param name="targetProperty">The target <see cref="DependencyProperty"/>.</param>
        /// <param name="targetObject">The target <see cref="DependencyObject"/>.</param>
        public LocalizationPropertyBindingSource(DependencyProperty targetProperty, DependencyObject targetObject)
        {
            // we don't check whether the objects are null for internal classes
            this.targetProperty = targetProperty;
            this.targetObject = targetObject;

            this.bindng = new Lazy<Binding>(this.CreateBinding);

            this.localizationPropertyChangedEvents = (WeakDependencyPropertyEventBus)targetObject.GetValue(LocalizationProperties.LocalizationPropertyChangedEventsProperty);

            this.localizationPropertyChangedEventsSubscription = this.localizationPropertyChangedEvents.CreateSubscription(this.OnDependencyPropertyEvent);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LocalizationPropertyBindingSource"/> class.
        /// </summary>
        ~LocalizationPropertyBindingSource()
        {
            this.Dispose();
            Debug.WriteLine(string.Format("~{0}", typeof(LocalizationPropertyBindingSource)));
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the value of the bound <see cref="DependencyProperty"/>.
        /// </summary>
        /// <value>
        /// The value of the bound <see cref="DependencyProperty"/>.
        /// </value>
        public object Value
        {
            get
            {
                return this.targetObject.GetValue(this.targetProperty);
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Data.Binding"/> for a given <see cref="DependencyProperty"/>.
        /// </summary>
        /// <value>
        /// The <see cref="System.Windows.Data.Binding"/> for a given <see cref="DependencyProperty"/>.
        /// </value>
        public Binding Binding
        {
            get
            {
                return this.bindng.Value;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!this.disposed)
            {
                using (this.localizationPropertyChangedEventsSubscription)
                {
                    this.disposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        private void OnDependencyPropertyEvent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == this.targetProperty)
            {
                PropertyChangedEventHandler propertyChanged = this.PropertyChanged;

                if (propertyChanged != null)
                {
                    propertyChanged(this, new PropertyChangedEventArgs(BoundPropertyName));
                }
            }
        }

        /// <summary>
        /// Creates the <see cref="System.Windows.Data.Binding"/> for the <see cref="Value"/> property of this instance.
        /// </summary>
        /// <returns>The <see cref="System.Windows.Data.Binding"/> for the <see cref="Value"/> property of this instance.</returns>
        private Binding CreateBinding()
        {
            return new Binding(BoundPropertyName) { Mode = BindingMode.OneWay, Source = this };
        }
    }
}
