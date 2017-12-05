using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQl.RootProvider
{
    internal class GraphQlExecuterProvider
    {
        private IDocumentExecuter Executer { get; }
        private IDocumentWriter Writer { get; }
        private Schema RootSchema { get; }
        public GraphQlExecuterProvider(Schema schema)
        {
            this.Executer = new DocumentExecuter();
            this.Writer = new DocumentWriter();
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

        public string Write(ExecutionResult result)
        {
            return this.Writer.Write(result);
        }
    }
}
