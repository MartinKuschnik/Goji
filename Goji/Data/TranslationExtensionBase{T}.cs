namespace Goji.Data
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows.Markup;

    /// <summary>
    /// A base class with functions for the various markup extension which can be used to localize properties of UI elements.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Key"/>-property.</typeparam>
    /// <remarks>
    ///
    /// We have 3 different kinds of translations:
    ///     1.  StaticTranslation:
    ///         A static string will be translated only on time.
    ///         The displayed translation will never change.
    ///     2.  DynamicTranslation:
    ///         A static string will be translated multiple times.
    ///         The displayed translation will updated if the language or the translator has changed.
    ///     3.  BindingTranslation:
    ///         A dynamic translation based on a binding as translation key.
    ///         The displayed translation will be updated if the binding, the language or the translator has changed.
    ///
    /// +-------------------------------------------------------+-------+-------------------------------------------------------+
    /// |-------------------------------------------------------|RUNTIME|-------------------------------------------------------|
    /// +-------------------------+--------------------------+----------------------------+-------------------------------------+
    /// |     translator type     |     language changed     |     translator changed     |    key changed (binding updated)    |
    /// |           #1            |             0            |              0             |                  0                  |
    /// |           #2            |             N            |              N             |                  0                  |
    /// |           #3            |             N            |              N             |                  N                  |
    /// +-------------------------+--------------------------+----------------------------+-------------------------------------+
    /// |                                                       0 - never updated    1 - one time updated    N - always updated |
    /// +-----------------------------------------------------------------------------------------------------------------------+
    ///
    /// +---------------------------------------+---------------------------------------+---------------------------------------+
    /// |---------------------------------------|VS_WPF_DESIGNER_TIMING_ISSUE_WORKAROUND|---------------------------------------|
    /// | see: http://connect.microsoft.com/VisualStudio/feedback/details/816195/attached-properties-not-working-on-design-time |
    /// +-------------------------+--------------------------+----------------------------+-------------------------------------+
    /// |     translator type     |     language changed     |     translator changed     |    key changed (binding updated)    |
    /// |           #1            |             N            |              N             |                  0                  |
    /// |           #2            |             N            |              N             |                  0                  |
    /// |           #3            |             N            |              N             |                  N                  |
    /// +-------------------------+--------------------------+----------------------------+-------------------------------------+
    /// |                                                       0 - never updated    1 - one time updated    N - always updated |
    /// +-----------------------------------------------------------------------------------------------------------------------+
    ///
    /// </remarks>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class TranslationExtensionBase<T> : MarkupExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationExtensionBase{T}"/> class.
        /// </summary>
        public TranslationExtensionBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationExtensionBase{T}"/> class.
        /// </summary>
        /// <param name="key">The key which is used as placeholder for the translation-value.</param>
        public TranslationExtensionBase(T key)
        {
            this.Key = key;
        }

        /// <summary>
        /// Gets or sets the key which represents the requested translation.
        /// </summary>
        /// <value>
        /// The key which represents the requested translation.
        /// </value>
        [ConstructorArgument("key")]
        public T Key { get; set; }

        /// <summary>
        /// Gets or sets the language which is used for the translation.
        /// </summary>
        /// <value>
        /// The language which is used for the translation.
        /// </value>
        public XmlLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets the composite format string. {0} will be replaced with the provided translation.
        /// </summary>
        /// <value>
        /// The composite format string. {0} will be replaced with the provided translation.
        /// </value>
        public string StringFormat { get; set; }

        /// <summary>
        /// Gets or sets the value which is use when the translation provider is unable to return a translation.
        /// </summary>
        /// <value>
        /// The value which is use when the translation provider is unable to return a translation.
        /// </value>
        public string FallbackValue { get; set; }

        /// <summary>
        /// Gets or sets the translation provider which is used to get the translations.
        /// </summary>
        /// <value>
        /// The translation provider which is used to get the translations.
        /// </value>
        public ITranslationProvider TranslationProvider { get; set; }

        /// <summary>
        /// Returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (provideValueTarget != null && provideValueTarget.TargetObject.GetType().FullName == "System.Windows.SharedDp")
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Throws an <see cref="Exception"/> if the <see cref="Key"/> property is not specified.
        /// </summary>
        /// <exception cref="System.Exception">The <see cref="Key"/> property is not specified.</exception>
        protected internal void ThrowIfKeyNotSpecified()
        {
            if (this.Key == null)
            {
                throw new Exception("No translation key found. Use the constructor or the Key property to define one.");
            }
        }
    }
}
