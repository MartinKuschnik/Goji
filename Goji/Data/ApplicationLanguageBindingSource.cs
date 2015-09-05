namespace Goji.Data
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Defines an object which can be used to bind the language of an <see cref="Application"/> object over a <see cref="System.Windows.Data.Binding"/> to the UI.
    /// </summary>
    internal class ApplicationLanguageBindingSource : INotifyPropertyChanged
    {
        /// <summary>
        /// The name of the property that returns the current application language.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const string ApplicationLanguagePropertyName = "ApplicationLanguage";

        /// <summary>
        /// The application.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Application application;

        /// <summary>
        /// The Lazy&lt;Binding&gt; object which created the <see cref="System.Windows.Data.Binding"/> if it's needed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<Binding> bindng;

        /// <summary>
        /// The subscription for the event which is raised when the current UI culture has changed.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDisposable currentUICultureChangedEventSubscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLanguageBindingSource"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public ApplicationLanguageBindingSource(Application application)
        {
            // internal class -> no parameter validation
            this.application = application;

            this.bindng = new Lazy<Binding>(this.CreateBinding);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">The instance is already disposed.</exception>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (this.PropertyChangedInternal == null)
                {
                    this.currentUICultureChangedEventSubscription = this.application.RegisterForCurrentUICultureChangedEvent(this.OnCurrentUICultureChanged);
                }

                this.PropertyChangedInternal += value;
            }

            remove
            {
                this.PropertyChangedInternal -= value;

                // we use this little trick to avoid memory leaks
                if (this.PropertyChangedInternal == null)
                {
                    this.currentUICultureChangedEventSubscription.Dispose();
                    this.currentUICultureChangedEventSubscription = null;
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        private event PropertyChangedEventHandler PropertyChangedInternal;

        /// <summary>
        /// Gets the current application language.
        /// </summary>
        /// <value>
        /// The current application language.
        /// </value>
        public XmlLanguage ApplicationLanguage
        {
            get
            {
                return XmlLanguage.GetLanguage(this.application.GetCurrentUICulture().IetfLanguageTag);
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
        /// Creates the <see cref="System.Windows.Data.Binding"/> for the <see cref="ApplicationLanguage"/> property of this instance.
        /// </summary>
        /// <returns>The <see cref="System.Windows.Data.Binding"/> for the <see cref="ApplicationLanguage"/> property of this instance.</returns>
        private Binding CreateBinding()
        {
            return new Binding(ApplicationLanguagePropertyName) { Mode = BindingMode.OneWay, Source = this };
        }

        /// <summary>
        /// Called when the current UI culture has changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="CultureChangedEventArgs"/> instance containing the event data.</param>
        private void OnCurrentUICultureChanged(object sender, CultureChangedEventArgs e)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChangedInternal;

            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(ApplicationLanguagePropertyName));
            }
        }
    }
}
