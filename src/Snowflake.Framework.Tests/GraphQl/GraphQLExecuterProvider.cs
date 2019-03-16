using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Snowflake.Support.Remoting.GraphQL.RootProvider;

namespace Snowflake.GraphQl.Tests
{
    internal class GraphQlTestExecuterProvider
    {
        private IDocumentExecuter Executer { get; }
        private Schema RootSchema { get; }

        public GraphQlTestExecuterProvider(Schema schema)
        {
            this.Executer = new DocumentExecuter();
            this.RootSchema = schema;
        }

        public async Task<ExecutionResult> ExecuteRequestAsync(GraphQlRequest request)
        {
            return await this.Executer.ExecuteAsync(_ =>
            {
                _.Schema = this.RootSchema;
                _.Query = request.Query;
                _.OperationName = request.OperationName;
                _.Inputs = request.Variables.ToInputs();
            });
        }
    }
}
