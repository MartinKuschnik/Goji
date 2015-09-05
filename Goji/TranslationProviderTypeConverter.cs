namespace Goji
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides multiple conversions from <see cref="string"/> to various translation providers.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class TranslationProviderTypeConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="System.Type"/> that represents the type you want to convert to.</param>
        /// <returns><c>true</c>, if this converter can perform the conversion, otherwise <c>false</c>.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(ITranslationProvider);
        }

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="System.Type"/> that represents the type you want to convert from.</param>
        /// <returns><c>true</c>, if this converter can perform the conversion, otherwise <c>false</c>.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="System.Globalization.CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="object"/> to convert.</param>
        /// <returns>An <see cref="object"/> that represents the converted value.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                const string typeDelimiter = "://";

                string[] valueParts = (value as string).Split(new string[] { typeDelimiter }, 2, StringSplitOptions.None);

                if (valueParts.Length < 2)
                {
                    throw new FormatException(string.Format("Invalid translation provider path. The path needs to start with 'xyz{0}'.", typeDelimiter));
                }

                switch (valueParts[0])
                {
                    case "resx":

                        Regex regex = new Regex("^([^;]+);([^;]+)$");

                        MatchCollection matches = regex.Matches(valueParts[1]);

                        if (matches.Count != 1)
                        {
                            throw new FormatException(string.Format("'{0}' i not a valid {1} translation provider path. The path needs to have the following format: '{1}{2}Assembly;FullClassName'.", value as string, valueParts[0], typeDelimiter));
                        }

                        string assemblyName = matches[0].Groups[1].Value;
                        string path = matches[0].Groups[2].Value;

                        return new ResxTranslationProvider(path, Assembly.Load(assemblyName));

                    default:
                        throw new NotSupportedException(string.Format("Invalid translation provider path. There is no translation provider type '{0}' defined.", valueParts[0]));
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
