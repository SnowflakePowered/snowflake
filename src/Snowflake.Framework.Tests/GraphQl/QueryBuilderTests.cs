using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.GraphQl
{
    public class QueryBuilderTests
    {
        public void MakeDelegateTest()
        {
            new BasicQueryBuilder()
        }
    }

    public class BasicQueryBuilder : QueryBuilder
    {
        public BasicQueryBuilder()
        {

        }
    }
}
