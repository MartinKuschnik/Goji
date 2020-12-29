namespace Goji
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// This converter is used by the bindings created by the various translation extensions.
    /// </summary>
    internal sealed class TranslationConverter : IValueConverter, IMultiValueConverter
    {
        /// <summary>
        /// This is the fallback value which is used if there is no translation for the given language.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string fallbackValue;

        /// <summary>
        /// The translation provider which should be used to get the translations.
        /// <para />
        /// This field can also be <c>null</c>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ITranslationProvider fixedTranslator;

        /// <summary>
        /// The language which is used to get a translation.
        /// <para />
        /// This field can also be <c>null</c>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly XmlLanguage fixedLanguage;

        /// <summary>
        /// This is the object will be used to determinate the used translation provider and the target language if the <see cref="fixedTranslator"/>
        /// or the <see cref="fixedLanguage"/> field is <c>null</c>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DependencyObject targetObject;

        /// <summary>
        /// A composite format string. {0} will be replaced with the provided translation.
        /// <para />
        /// This field can also be <c>null</c> or an empty string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string stringFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationConverter"/> class.
        /// </summary>
        /// <param name="targetObject">
        /// This is the object will be used to determinate the used translation provider and the target language if the <paramref name="fixedTranslator"/>
        /// or the <paramref name="fixedLanguage"/> parameter is <c>null</c>.
        /// </param>
        /// <param name="fixedTranslator">
        /// The translation provider which is used to get a translation.
        /// <para />
        /// If this parameter is <c>null</c>, the instance will use the target object to determinate the used translation provider.</param>
        /// <param name="fixedLanguage">
        /// The language which is used to get a translation.
        /// <para />
        /// If this parameter is <c>null</c>, the instance will use the target object to determinate the target language.
        /// </param>
        /// <param name="stringFormat">A composite format string. {0} will be replaced with the provided translation.</param>
        /// <param name="fallbackValue">This is the fallback value which is used if there is no translation for the given language.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="targetObject"/> parameter is <c>null</c>.</exception>
        internal TranslationConverter(DependencyObject targetObject, ITranslationProvider fixedTranslator = null, XmlLanguage fixedLanguage = null, string stringFormat = null, string fallbackValue = null)
        {
            if (targetObject == null)
            {
                throw new ArgumentNullException(MethodBase.GetCurrentMethod().GetParameters()[0].Name);
            }

            this.targetObject = targetObject;
            this.fixedTranslator = fixedTranslator;
            this.fixedLanguage = fixedLanguage;
            this.stringFormat = stringFormat;
            this.fallbackValue = fallbackValue;
        }

        /// <summary>
        /// Converts an given translation-key into the corresponding translation.
        /// </summary>
        /// <param name="value">The translation-key who should be translated.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The language of the requested translation.</param>
        /// <returns>The translation for the given translation-key and language.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ITranslationProvider usedTranslator = this.fixedTranslator;
            CultureInfo usedCulture = culture;
            string fallbackValue = this.fallbackValue ?? value.ToString();

            if (usedTranslator == null)
            {
                usedTranslator = this.targetObject.GetValue(LocalizationProperties.TranslationProviderProperty) as ITranslationProvider;

                if (usedTranslator == null)
                {
                    if (string.IsNullOrEmpty(this.stringFormat))
                    {
                        return fallbackValue;
                    }
                    else
                    {
                        return string.Format(this.stringFormat, fallbackValue);
                    }
                }
            }

            if (this.fixedLanguage != null)
            {
                usedCulture = new CultureInfo(this.fixedLanguage.ToString());
            }

            return usedTranslator.ProvideTranslation(value.ToString(), usedCulture, fallbackValue, this.stringFormat);
        }

        /// <summary>
        /// Converts an translation into the corresponding translation-key.
        /// </summary>
        /// <param name="value">The translation.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The language of the translation.</param>
        /// <returns>The corresponding translation-key for the given translation.</returns>
        /// <exception cref="NotSupportedException">Backward translation is not supported.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Converts an given translation-key into the corresponding translation.
        /// </summary>
        /// <param name="values">The translation-keys who should be translated.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The language of the requested translation.</param>
        /// <returns>The translation for the given translation-key and language.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!values.Any() || (!(values[0] is string) && values[0].GetType().FullName == "MS.Internal.NamedObject"))
            {
                return null;
            }

            return this.Convert(values[0], targetType, parameter, culture);
        }

        /// <summary>
        /// Converts an translation into the corresponding translation-key.
        /// </summary>
        /// <param name="value">The translation.</param>
        /// <param name="targetTypes">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The language of the translation.</param>
        /// <returns>The corresponding translation-key for the given translation.</returns>
        /// <exception cref="NotSupportedException">Backward translation is not supported.</exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}