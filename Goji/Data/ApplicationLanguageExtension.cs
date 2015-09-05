namespace Goji.Data
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Can be used to bind the application language to the <see cref="FrameworkElement.LanguageProperty"/>.
    /// </summary>
    [MarkupExtensionReturnType(typeof(XmlLanguage))]
    public class ApplicationLanguageExtension : MarkupExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationLanguageExtension"/> class.
        /// </summary>
        public ApplicationLanguageExtension()
        {
        }

        /// <summary>
        /// Returns an object that is provided as the current application language.
        /// </summary>
        /// <param name="serviceProvider">>A service provider helper that can provide services for the markup extension.</param>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            ApplicationLanguageBindingSource bindingSource = new ApplicationLanguageBindingSource(Application.Current);

            // Creates a simple binding.
            Binding binding = bindingSource.Binding;

            return binding.ProvideValue(serviceProvider);
        }
    }
}
