using System.Reflection;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OfuscationProofCodestylePlugin")]
[assembly: AssemblyDescription("Inspects code to make sure it follows obfuscation-proof patterns. Provides quick fixes to found errors.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Abbyy Language Services")]
[assembly: AssemblyProduct("Obfuscation-proof codestyle ReSharper plugin")]
[assembly: AssemblyCopyright("Copyright © Abbyy Language Services, 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.3.0.0")]
[assembly: AssemblyFileVersion("1.3.0.0")]

[assembly: ActionsXml("OfuscationProofCodestylePlugin.Actions.xml")]

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("Obfuscation-proof codestyle")]
[assembly: PluginDescription("Inspects code to make sure it follows obfuscation-proof patterns. Provides quick fixes to found errors.")]
[assembly: PluginVendor("Abbyy Language Services")]
