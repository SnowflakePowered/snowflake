var najax = require("najax");
var Promise = require('promise');
var Snowflake = function Snowflake(snowflakeUrl) {
    var instance = this;

    this.snowflakeUrl = snowflakeUrl;
    this.platforms = {};
    this.games = {};
    this.loadGames = function () {
        return new Promise(function (resolve, reject) {
            this.ajax('', '', function (response) { resolve(response); });
        });
    };
    this.loadPlatforms = function () {
    };

    this.ajax = function (namespace, methodname, callback) {
        return najax('http://www.xgoogle.com', callback);
    }

}

exports.Snowflake = Snowflake;