namespace Goji
{
    using System;
    using System.Diagnostics;
    using System.Windows;

    /// <summary>
    /// Defines different <see cref="DependencyProperty"/>s for localization purposes.
    /// </summary>
    public static class LocalizationProperties
    {
        /// <summary>
        /// Identifies the <c>TranslationProvider</c> dependency property.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static readonly DependencyProperty TranslationProviderProperty
            = DependencyProperty.RegisterAttached(
                "TranslationProvider",
                typeof(ITranslationProvider),
                typeof(LocalizationProperties),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, OnTranslationProviderPropertyChanged));

        /// <summary>
        /// Identifies the <c>LocalizationPropertyChangedEvents</c> dependency property.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly DependencyProperty LocalizationPropertyChangedEventsProperty
            = DependencyProperty.RegisterAttachedReadOnly(
                "LocalizationPropertyChangedEvents",
                typeof(WeakDependencyPropertyEventBus),
                typeof(LocalizationProperties),
                new FrameworkPropertyMetadata(new WeakDependencyPropertyEventBus())).DependencyProperty;

        /// <summary>
        /// Sets the <c>TranslationProvider</c> dependency property.
        /// </summary>
        /// <param name="dp">The <see cref="UIElement"/>.</param>
        /// <param name="value">The <see cref="ITranslationProvider"/> object which should be set.</param>
        public static void SetTranslationProvider(UIElement dp, ITranslationProvider value)
        {
            dp.SetValue(TranslationProviderProperty, value);
        }

        /// <summary>
        /// Gets the <c>TranslationProvider</c> dependency property.
        /// </summary>
        /// <param name="dp">The <see cref="UIElement"/>.</param>
        /// <returns>The current <see cref="ITranslationProvider"/> object.</returns>
        public static ITranslationProvider GetTranslationProvider(UIElement dp)
        {
            return (ITranslationProvider)dp.GetValue(TranslationProviderProperty);
        }

        /// <summary>
        /// Called when <see cref="TranslationProviderProperty"/> has changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTranslationProviderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            WeakDependencyPropertyEventBus events = d.GetValue(LocalizationPropertyChangedEventsProperty) as WeakDependencyPropertyEventBus;

            events.NotifySubscribers(d, args);
        }
    }
}
