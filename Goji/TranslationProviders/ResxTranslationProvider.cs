namespace Goji
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    /// <summary>
    /// A translation provider which uses .resx-files as source.
    /// </summary>
    internal sealed class ResxTranslationProvider : ITranslationProvider
    {
        /// <summary>
        /// The internally used <see cref="ResourceManager"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ResourceManager resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResxTranslationProvider"/> class.
        /// </summary>
        /// <param name="baseName">
        /// The root name of the resource file without its extension but including any fully
        /// qualified namespace name. For example, the root name for the resource file named
        /// MyApplication.MyResource.en-US.resources is MyApplication.MyResource.
        /// </param>
        /// <param name="assembly">The main assembly for the resources.</param>
        /// <exception cref="System.Resources.MissingManifestResourceException">Could not find any resources for the given <paramref name="baseName"/>.</exception>
        public ResxTranslationProvider(string baseName, Assembly assembly)
        {
            // no parameter validation for internal types
            this.resourceManager = new ResourceManager(baseName, assembly);

            try
            {
                // we only do this call to validate the given baseName
                this.resourceManager.GetString(string.Empty);
            }
            catch (MissingManifestResourceException)
            {
                // this exception is thrown if the path is completely wrong
                // let's improve the exception message a bit
                throw new MissingManifestResourceException(string.Format("Could not find any resources. Make sure \"{0}.resources\" was correctly embedded or linked into assembly \"{1}\" at compile time, or that all the satellite assemblies required are loadable and fully signed.", baseName, assembly.GetName().Name));
            }
        }

        /// <summary>
        /// Provides a translation for a given key and culture.
        /// </summary>
        /// <param name="key">The key that represents the requested translation.</param>
        /// <param name="culture">The culture for which the translation should be provided.</param>
        /// <returns>The translated value or <c>null</c> if no translation could be found.</returns>
        public string ProvideTranslation(string key, CultureInfo culture)
        {
            return this.resourceManager.GetString(key, culture);
        }
    }
}