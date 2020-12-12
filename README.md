# Goji  [![Build status](https://ci.appveyor.com/api/projects/status/i3w5vb5q8p19t70l?svg=true)](https://ci.appveyor.com/project/martinkuschnik/goji) [![NuGet Status](http://img.shields.io/nuget/v/Goji.svg?style=flat)](https://www.nuget.org/packages/Goji/)
A framework to localize WPF applications.

## What's Goji?

It's a framework that helps you to localize your WPF applications. Therefore it provides different kinds of markup extensions.

&emsp;&emsp;![Example](https://raw.githubusercontent.com/MartinKuschnik/Goji/master/doc/pics/example.gif)


## Are there any requirements?

Goji can be used for all WPF applications which targeting to the .Net  Framework 4 or any higher version. 

## License

Goji is licensed under The GNU GENERAL PUBLIC LICENSE v3, check the LICENSE file for details.

## Installation

## Getting Started 

**Step 1:** Create a new WPF application or open an existing one.

**Step 2:** Install the NuGet-Package.

<a href="https://www.nuget.org/packages/Goji/" target="_blank">
&emsp;&nbsp;&nbsp;&nbsp;<img title="NuGet" src="https://github.com/MartinKuschnik/Goji/blob/master/doc/pics/install_nuget_package.JPG" alt="NuGet"/>
</a>

**Step 3:** Bind a view to the application language.
  
&emsp;&emsp;![ApplicationLanguage](https://raw.githubusercontent.com/MartinKuschnik/Goji/master/doc/pics/ApplicationLanguage.PNG)

**Step 4:** Add the translation provider files you'd like to use.

&emsp;&emsp;![RESX](https://raw.githubusercontent.com/MartinKuschnik/Goji/master/doc/pics/added_resx_files.PNG)


**Step 6:** Set the translation provider for the UI element you'd like to localize.

&emsp;&emsp;![Set Translation Provider](https://raw.githubusercontent.com/MartinKuschnik/Goji/master/doc/pics/set_translation_provider.png)

**Step 6:** Use one of the translation extensions to localize a string.

&emsp;&emsp;![Static Translation](https://raw.githubusercontent.com/MartinKuschnik/Goji/master/doc/pics/static_translation.png)

## What does the ApplicationLanguage markup extension?

This extension makes it very easy to synchronize the language of a control with the applicatuion language. The language of the control is automaticaly updated if you change the application language by using the extension method ```Application.SetCurrentUICulture(CultureInfo)```. This will update the formatting and the translations.

  Usage:
  ```
  <Control Language="{ApplicationLanguage}" />
  ```
## Which kind of translation providers are supported?

...
  
## Why multiple markup extension for translation purposes?

Localizing an application will create different kinds of situations with variant requirements. Therefore there are variant markup extensions with different levels of complexity.

- StaticTranslation

  This is the most rudimentary way to localize a text based property.
  The translation is done only once.
  
  *Use Case: Use this type of translation for dialog based views which prevent the user to change the language / culture and are recreated every time.*
  
- DynamicTranslation

  This markup extension automatically triggers a retranslation when the control language has changed. 
  
  *Use Case: This one should be used if the displayed value should always be up to date. Use this markup extension if you are not sure whether the view stays alive during a language change.*
  
- BindingTranslation

  The BindingTranslation is an extended version of the DynamicTranslation and  retranslates the bound key if this has changed.
  
  *Use Case: Use this one if you'd like to show a localized text based on an enumeration provided by the view model..*

The following table should highlight the differences a bit more:

|Markup Extension|Updates on language or translator changed?|Updates on translation key changed?|
|---|---|---|
|StaticTranslation|No |No |
|DynamicTranslation|Yes|No |
|BindingTranslation|Yes|Yes|

