(function(){
	var snowflakeModule = require("./snowflake.js");
	
	var Snowflake = snowflakeModule.Snowflake;
	if(global.snowflake === undefined){
			global.snowflake = {};
			global.snowflake.snowflakeApi = new Snowflake("http://localhost:30001");
	}
})();
exports._sflk = function(){
	window.snowflake = global.snowflake;
}