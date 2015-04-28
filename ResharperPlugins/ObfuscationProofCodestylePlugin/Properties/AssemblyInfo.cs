using System.Reflection;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OfuscationCodestylePlugin")]
[assembly: AssemblyDescription("Inspects code to make sure it follows obfuscation-proof patterns. Provides quick fixes to found errors.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Abbyy Language Services")]
[assembly: AssemblyProduct("Obfuscation codestyle ReSharper plugin")]
[assembly: AssemblyCopyright("Copyright © Abbyy Language Services, 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]

[assembly: ActionsXml("OfuscationCodestylePlugin.Actions.xml")]

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("Obfuscation codestyle")]
[assembly: PluginDescription("Inspects code to make sure it follows obfuscation-proof patterns. Provides quick fixes to found errors.")]
[assembly: PluginVendor("Abbyy Language Services")]
