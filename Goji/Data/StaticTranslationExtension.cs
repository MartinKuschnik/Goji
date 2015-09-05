namespace Goji.Data
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    /// <summary>
    /// Can be used to localize properties of UI elements.
    /// <para />
    /// Use this markup extension if your translation key is a simple <see cref="string"/>
    /// and the value should not be updated if the language property of the UI element changes.
    /// <para />
    /// Recommended for UI elements with short lifetime.
    /// </summary>
    [MarkupExtensionReturnType(typeof(string))]
    public sealed class StaticTranslationExtension : TranslationExtensionBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StaticTranslationExtension"/> class.
        /// </summary>
        public StaticTranslationExtension()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticTranslationExtension"/> class.
        /// </summary>
        /// <param name="key">The key which is used as placeholder for the translation-value.</param>
        public StaticTranslationExtension(string key)
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

            IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (provideValueTarget != null)
            {
                DependencyObject targetObject = provideValueTarget.TargetObject as DependencyObject;

                if (targetObject != null)
                {
                    var translator = this.TranslationProvider ?? targetObject.GetValue(LocalizationProperties.TranslationProviderProperty) as ITranslationProvider;

                    if (translator != null)
                    {
                        return new TranslationConverter(targetObject, translator, this.Language ?? (XmlLanguage)targetObject.GetValue(Control.LanguageProperty), this.StringFormat, this.FallbackValue).Convert(this.Key, null, null, null);
                    }
#if VS_WPF_DESIGNER_TIMING_ISSUE_WORKAROUND // see: http://connect.microsoft.com/VisualStudio/feedback/details/816195/attached-properties-not-working-on-design-time
                    else if (DesignerProperties.GetIsInDesignMode(targetObject))
                    {
                        DynamicTranslationExtension dynamicOne = new DynamicTranslationExtension();

                        // transfer properties
                        dynamicOne.Key = this.Key;
                        dynamicOne.Language = this.Language;
                        dynamicOne.StringFormat = this.StringFormat;
                        dynamicOne.TranslationProvider = this.TranslationProvider;
                        dynamicOne.FallbackValue = this.FallbackValue;

                        return dynamicOne.ProvideValue(serviceProvider);
                    }
#endif
                }
            }

            return base.ProvideValue(serviceProvider);
        }
    }
}