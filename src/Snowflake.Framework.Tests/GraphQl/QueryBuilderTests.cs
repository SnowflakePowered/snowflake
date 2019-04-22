using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;
using Snowflake.Framework.Remoting.GraphQL.Attributes;
using Snowflake.Framework.Remoting.GraphQL.Query;
using Snowflake.Services.Logging;
using Snowflake.Support.Remoting.GraphQL.RootProvider;
using Xunit;

namespace Snowflake.GraphQl
{
    public class QueryBuilderTests
    {
        [Fact]
        public void RegisterFieldQuery_Test()
        {
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            var queryBuilder = new BasicQueryBuilder();
            queryBuilder.RegisterFieldQueries(root);
            Assert.True(schema.Query.HasField("defaultTest"));
        }

        [Fact]
        public void RegisterConnectionQuery_Test()
        {
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            var queryBuilder = new BasicQueryBuilder();
            queryBuilder.RegisterConnectionQueries(root);
            Assert.True(schema.Query.HasField("connectionTest"));
        }

        [Fact]
        public void RegisterMutationQuery_Test()
        {
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            var queryBuilder = new BasicQueryBuilder();
            queryBuilder.RegisterMutationQueries(mutation);
            Assert.True(schema.Mutation.HasField("mutationTest"));
        }

        [Fact]
        public void RegisterAll_Test()
        {
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            var queryBuilder = new BasicQueryBuilder();
            schema.Register(queryBuilder);
            Assert.True(schema.Query.HasField("defaultTest"));
            Assert.True(schema.Query.HasField("connectionTest"));
            Assert.True(schema.Mutation.HasField("mutationTest"));
        }

        [Fact]
        public void RegisterBroken_Test()
        {
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            var queryBuilder = new BrokenQueryBuilder();
            Assert.Throws<ArgumentOutOfRangeException>(() => schema.Register(queryBuilder));
        }

        [Fact]
        public void DefaultValue_Test()
        {
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            var queryBuilder = new BasicQueryBuilder();
            schema.Register(queryBuilder);
            var resolved = schema.Query.Fields.First(p => p.Name == "defaultTest").Resolver.Resolve(
                new ResolveFieldContext()
                {
                    Arguments = new Dictionary<string, object>()
                    {
                        {"returnOne", "Foo Bar and Eggs"},
                    },
                });

            Assert.Equal("Foo Bar and EggsHello World", resolved);

            var resolvedTwo = schema.Query.Fields.First(p => p.Name == "defaultTest").Resolver.Resolve(
                new ResolveFieldContext()
                {
                    Arguments = new Dictionary<string, object>()
                    {
                        {"returnOne", "One"},
                        {"returnTwo", "Two"},
                    },
                });

            Assert.Equal("OneTwo", resolvedTwo);
        }

        [Fact]
        public void ConnectionValue_Test()
        {
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            var queryBuilder = new BasicQueryBuilder();
            schema.Register(queryBuilder);
            var actual = (Connection<string>) schema.Query.Fields.First(p => p.Name == "connectionTest").Resolver
                .Resolve(new ResolveFieldContext()
                {
                    Arguments = new Dictionary<string, object>()
                    {
                        {"returnOne", "One"},
                        {"returnTwo", "Two"},
                    },
                });

            IEnumerable<string> Expected()
            {
                yield return "One";
                yield return "Two";
            }

            Assert.True(!Expected().Except(actual.Items).Any() && Expected().Count() == actual.Items.Count());
        }

        [Fact]
        public void MutationValue_Test()
        {
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            var queryBuilder = new BasicQueryBuilder();
            schema.Register(queryBuilder);
            var actual = schema.Mutation.Fields.First(p => p.Name == "mutationTest").Resolver.Resolve(
                new ResolveFieldContext()
                {
                    Arguments = new Dictionary<string, object>()
                    {
                        {
                            "input", new TestInputType()
                            {
                                Input = "Hello World",
                            }
                        },
                    },
                });
            Assert.Equal("Hello World", actual);
        }

        [Fact]
        public async Task GraphQLFieldQuery_Test()
        {
            var root = new RootQuery();
            var mutation = new RootMutation();
            var schema = new GraphQlRootSchema(root, mutation);
            var queryBuilder = new BasicQueryBuilder();
            var executor = new GraphQlExecuterProvider(schema);
            schema.Register(queryBuilder);
            var result = await executor.ExecuteRequestAsync(new GraphQlRequest()
            {
                Query = @"query {
                            defaultTest(returnOne: ""Hello"", returnTwo: ""World"")
                        }",
            });

            Assert.Equal("HelloWorld", ((dynamic) result.Data)["defaultTest"]);
        }
    }

    public class BrokenQueryBuilder : QueryBuilder
    {
        [Field("broken", "", typeof(StringGraphType))]
        public string Broken(string brokenParam)
        {
            return brokenParam;
        }
    }

    public class BasicQueryBuilder : QueryBuilder
    {
        [Connection("connectionTest", "", typeof(StringGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "returnOne", "")]
        [Parameter(typeof(string), typeof(StringGraphType), "returnTwo", "")]
        public IEnumerable<string> ConnectionTest(string returnOne, string returnTwo)
        {
            yield return returnOne;
            yield return returnTwo;
        }

        [Field("defaultTest", "", typeof(StringGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "returnOne", "")]
        [Parameter(typeof(string), typeof(StringGraphType), "returnTwo", "")]
        public string DefaultTest(string returnOne, string returnTwo = "Hello World")
        {
            return returnOne + returnTwo;
        }

        [Mutation("mutationTest", "", typeof(StringGraphType))]
        [Parameter(typeof(TestInputType), typeof(TestInputGraphType), "input", "")]
        public string MutationTest(TestInputType input)
        {
            return input.Input;
        }
    }

    public class TestInputType
    {
        public string Input { get; set; }
    }

    public class TestInputGraphType : InputObjectGraphType<TestInputType>
    {
        public TestInputGraphType()
        {
            Field(p => p.Input);
        }
    }
}
