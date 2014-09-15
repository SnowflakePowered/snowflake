(function(){
	var snowflakeModule = require("./snowflake.api.js");
	var snowflakeConstants = require("./snowflake.constants.js");

	var Snowflake = snowflakeModule.Snowflake;
	if(global.snowflake === undefined){
			global.snowflake = {};
			global.snowflake.api = new Snowflake("http://localhost:30001");
			global.snowflake.constants = snowflakeConstants;
	}
})();
exports._sflk = function(){
	window.snowflake = global.snowflake;
}