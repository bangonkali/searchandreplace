using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany("Bangon Kali")]
[assembly: AssemblyCopyright("Copyright © 2017 Bangon Kali")]
[assembly: AssemblyProduct("SearchAndReplace")]
[assembly: AssemblyTrademark("SearchAndReplace")]
[assembly: AssemblyTitle("SearchAndReplace")]
[assembly: AssemblyDescription("A very simple search and replace tool that does search and replace on folder names, file names and contents of files within the root folder and doing so recursively.")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("6841bee6-4d2f-4d91-b93d-1d9a16e36b7c")]
