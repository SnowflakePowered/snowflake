(function(){
	var snowflakeModule = require("./snowflake.js");
	
	var Snowflake = snowflakeModule.Snowflake;
	if(global.snowflake === undefined){
			global.snowflake = {};
			global.snowflake.snowflakeApi = new Snowflake("http://localhost:30001");
	}
	var gui = require('nw.gui'); //or global.window.nwDispatcher.requireNwGui() (see https://github.com/rogerwang/node-webkit/issues/707)
	var win = gui.Window.get();
	win.on('loaded', function(){
		win.window.snowflake = global.snowflake;
	});
})();
exports._sflk = function(){
	window.snowflake = global.snowflake;
}