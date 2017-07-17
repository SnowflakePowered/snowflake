using Snowflake.Remoting.Marshalling;
using Snowflake.Remoting.Requests;
using Snowflake.Remoting.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
namespace Snowflake.Remoting
{
    public class ResourceRemotingTests
    {
        [Fact]
        public void ResourceEnum_Test()
        {
            var res = new DummyResource();

        }

        [Fact]
        public void Match_Test()
        {
            var res = new DummyResource();
            var _req = "game/foobar";
            var req = new RequestPath(_req.Split("/"));
            Assert.True(res.Path.Match(req));
            var __req = "game/foobar/bazeggs";
            var badreq = new RequestPath(__req.Split("/"));
            Assert.False(res.Path.Match(badreq));
        }

        [Fact]
        public void MatchResArgs_Test()
        {
            var res = new DummyResource();
            var _req = "game/foobar";
            var req = new RequestPath(_req.Split("/"));
            var args = res.Path.SerializePathArguments(req).ToList();
            Assert.Contains(args, p => p.StringValue == "foobar");
        }

        [Fact]
        public void MatchEmptyParams_Test()
        {
            var res = new DummyResource();
            var mInfo = res.GetType().GetRuntimeMethod("Get", Type.EmptyTypes);
            var _req = "game/foobar";
            var req = new RequestPath(_req.Split("/"));
            var param = Enumerable.Empty<EndpointArgument>();
            var matched = res.MatchEndpointWithParams(EndpointVerb.Read, param);
            Assert.Equal(mInfo, matched.EndpointMethodInfo);
        }

        [Fact]
        public void MatchWithParams_Test()
        {
            var res = new DummyResource();
            var mInfo = res.GetType().GetRuntimeMethod("Echo", new[] { typeof(string), typeof(string) });
            var _req = "game/foobar";
            var req = new RequestPath(_req.Split("/"));
            var param = new[]
            {
                new EndpointArgument("echoText", "Some Echo")
            };

            var matched = res.MatchEndpointWithParams(EndpointVerb.Create, param);
            Assert.Equal(mInfo, matched.EndpointMethodInfo);
        }

        [Fact]
        public void MatchWithParamsOverload_Test()
        {
            var res = new DummyResource();
            var mInfo = res.GetType().GetRuntimeMethod("Echo", new[] { typeof(string), typeof(string), typeof(string) });
            var _req = "game/foobar";
            var req = new RequestPath(_req.Split("/"));
            var param = new[]
            {
                new EndpointArgument("echo", "0"),
                new EndpointArgument("echoText", "Some Echo"),
                new EndpointArgument("someOther", "helloWorld")
            };

            var matched = res.MatchEndpointWithParams(EndpointVerb.Create, param);
            Assert.Null(matched);
        }

        [Fact]
        public void MatchWithCastedParams_Test()
        {
            var res = new DummyResource();
            var mInfo = res.GetType().GetRuntimeMethod("Echo", new[] { typeof(string), typeof(string), typeof(string) });
            var _req = "game/foobar";
            var req = new RequestPath(_req.Split("/"));
            var param = new[]
            {
                new EndpointArgument("echoText", "Some Echo"),
                new EndpointArgument("someOther", "helloWorld")
            };

            var matched = res.MatchEndpointWithParams(EndpointVerb.Create, param);
            var pathArgs = res.Path.SerializePathArguments(req);
            var endsArgs = matched.SerializeEndpointArguments(param);
            var casted = ArgumentTypeMapper.Default.CastArguments(pathArgs, endsArgs);
        }


        [Fact]
        public void Exec_Test()
        {
            var res = new DummyResource();
            var mInfo = res.GetType().GetRuntimeMethod("Echo", new[] { typeof(string), typeof(string), typeof(string) });
            var _req = "game/foobar";
            var req = new RequestPath(_req.Split("/"));
            var param = new[]
            {
                new EndpointArgument("echoText", "Some Echo"),
                new EndpointArgument("someOther", "helloWorld")
            };

            var matched = res.MatchEndpointWithParams(EndpointVerb.Create, param);
            var pathArgs = res.Path.SerializePathArguments(req);
            var endsArgs = matched.SerializeEndpointArguments(param);
            var casted = ArgumentTypeMapper.Default.CastArguments(pathArgs, endsArgs);
            var result = res.Execute(matched, casted);
            Assert.Equal("foobarSome EchohelloWorld", (string)result);
        }

        [Fact]
        public void MatchWithCastedOverriddenParams_Test()
        {
            var res = new DummyResource();
            var mInfo = res.GetType().GetRuntimeMethod("Echo", new[] { typeof(string), typeof(string), typeof(string) });
            var _req = "game/foobar";
            var req = new RequestPath(_req.Split("/"));
            var param = new[]
            {
                new EndpointArgument("echoText", "Some Echo"),
                new EndpointArgument("someOther", "helloWorld")
            };

            var matched = res.MatchEndpointWithParams(EndpointVerb.Create, param);
            var pathArgs = res.Path.SerializePathArguments(req);
            var endsArgs = matched.SerializeEndpointArguments(param);
            var casted = ArgumentTypeMapper.Default.CastArguments(pathArgs, endsArgs);
            var result = res.Execute(matched, casted);
            Assert.Equal("foobarSome EchohelloWorld", (string)result);
        }

        [Fact]
        public void MatchWithMisMatchedResourceTypeParams_Test()
        {
            var res = new DummyResource();
            var _req = "game/foobar";
            var req = new RequestPath(_req.Split("/"));
            var param = new[]
            {
                new EndpointArgument("echoTextMisMatched", "Some Echo"),
                new EndpointArgument("someOther", "helloWorld")
            };

            var matched = res.MatchEndpointWithParams(EndpointVerb.Create, param);
            Assert.Null(matched);
        }
    }
}
