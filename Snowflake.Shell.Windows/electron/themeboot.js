var Snowflake, SnowflakeEndpoint, _ref;

_ref = require('snowflake'), SnowflakeEndpoint = _ref.SnowflakeEndpoint, Snowflake = _ref.Snowflake;

window.snowflake = new Snowflake(new SnowflakeEndpoint("ws://localhost:30003"));
