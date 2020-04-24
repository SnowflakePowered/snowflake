using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Testing purposes
[assembly: InternalsVisibleTo("Snowflake.Framework.Tests")]
[assembly: InternalsVisibleTo("Snowflake.Framework.Tests.GraphQL")]

// Internal input item types initialization
[assembly: InternalsVisibleTo("HotChocolate.Core")]

[assembly: ComVisible(false)]
