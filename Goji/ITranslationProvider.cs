namespace Goji
{
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// Defines a provider for translations.
    /// </summary>
    [TypeConverter(typeof(TranslationProviderTypeConverter))]
    public interface ITranslationProvider
    {
        /// <summary>
        /// Provides a translation for a given key and culture.
        /// </summary>
        /// <param name="key">The key that represents the requested translation.</param>
        /// <param name="culture">The culture for which the translation should be provided.</param>
        /// <returns>The translated value or <c>null</c> if no translation could be found.</returns>
        string ProvideTranslation(string key, CultureInfo culture);
    }
}
