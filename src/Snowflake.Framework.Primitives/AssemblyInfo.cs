using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCulture("")]

// Framework-hidden interface methods
[assembly: InternalsVisibleTo("Snowflake.Framework")]
[assembly: InternalsVisibleTo("Snowflake.Framework.Services")]
[assembly: InternalsVisibleTo("Snowflake.Support.GraphQL.Server")]

[assembly: InternalsVisibleTo("Snowflake.Bootstrap.Windows")]
[assembly: InternalsVisibleTo("Snowflake.Bootstrap.Linux")]

// Needed for mocking purposes
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("Snowflake.Framework.Tests")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
