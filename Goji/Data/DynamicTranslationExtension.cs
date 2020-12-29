namespace Goji.Data
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Can be used to localize properties of UI elements.
    /// <para />
    /// Use this markup extension if your translation key is a simple <see cref="string"/>
    /// and the value should be updated if the language property of the UI element changes.
    /// <para />
    /// Recommended for UI elements with short lifetime.
    /// </summary>
    [MarkupExtensionReturnType(typeof(string))]
    public sealed class DynamicTranslationExtension : TranslationExtensionBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicTranslationExtension"/> class.
        /// </summary>
        public DynamicTranslationExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicTranslationExtension"/> class.
        /// </summary>
        /// <param name="key">The key who is used as placeholder for the translation-value.</param>
        public DynamicTranslationExtension(string key)
            : base(key)
        {
        }

        /// <summary>
        /// Returns an object that is provided as the translation of the target translation-key.
        /// </summary>
        /// <param name="serviceProvider">>A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        /// <exception cref="System.Exception">The <c>Key</c> property is not specified.</exception>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            this.ThrowIfKeyNotSpecified();

            MultiBinding binding = new MultiBinding();

            binding.Bindings.Add(new Binding() { Source = this.Key });

            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (provideValueTarget != null)
            {
                if (provideValueTarget.TargetObject is DependencyObject targetObject)
                {
                    var bindingSource = new LocalizationPropertyBindingSource(LocalizationProperties.TranslationProviderProperty, targetObject);

                    binding.Bindings.Add(bindingSource.Binding);

                    binding.Converter = new TranslationConverter(targetObject, this.TranslationProvider, this.Language, this.StringFormat, this.FallbackValue);

                    return binding.ProvideValue(serviceProvider);
                }
                else
                {
                    return this;
                }
            }

            return base.ProvideValue(serviceProvider);
        }
    }
}