namespace Goji
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// This class defines extension methods for the <see cref="ITranslationProvider"/> type.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ITranslationProviderExtensions
    {
        /// <summary>
        /// Provides a translation for a given key and culture.
        /// </summary>
        /// <param name="translationProvider">The translation provider which is used for the operation.</param>
        /// <param name="key">The key that represents the requested translation.</param>
        /// <param name="culture">The culture for which the translation should be provided.</param>
        /// <param name="fallbackValue">This value is returned if there is no translation for the given key and culture.</param>
        /// <param name="stringFormat">A composite format string. {0} will be replaced with the provided translation.</param>
        /// <returns>The translated value, <c>null</c> or the value from the <paramref name="fallbackValue"/> parameter if no translation could be found.</returns>
        public static string ProvideTranslation(this ITranslationProvider translationProvider, string key, CultureInfo culture, string fallbackValue, string stringFormat)
        {
            if (string.IsNullOrEmpty(stringFormat))
            {
                return translationProvider.ProvideTranslation(key, culture, fallbackValue);
            }
            else
            {
                return string.Format(stringFormat, translationProvider.ProvideTranslation(key, culture, fallbackValue));
            }
        }

        /// <summary>
        /// Provides a translation for a given key and culture.
        /// </summary>
        /// <param name="translationProvider">The translation provider which is used for the operation.</param>
        /// <param name="key">The key that represents the requested translation.</param>
        /// <param name="culture">The culture for which the translation should be provided.</param>
        /// <param name="fallbackValue">This value is returned if there is no translation for the given key and culture.</param>
        /// <returns>The translated value, <c>null</c> or the value from the <paramref name="fallbackValue"/> parameter if no translation could be found.</returns>
        public static string ProvideTranslation(this ITranslationProvider translationProvider, string key, CultureInfo culture, string fallbackValue)
        {
            return translationProvider.ProvideTranslation(key, culture) ?? fallbackValue;
        }
    }
}
