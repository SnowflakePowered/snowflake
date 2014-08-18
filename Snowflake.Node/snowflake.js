var najax = require("najax");
var Promise = require('promise');
var Snowflake = function Snowflake(snowflakeUrl) {
    var instance = this;

    this.snowflakeUrl = snowflakeUrl;
    this.platforms = {};
    this.games = {};
    this.loadGames = function () {
        return this.ajax('', '').then(function (response) {
            instance.games = response;
        }).catch(function (fail) {
            console.log(fail);
        });
    };
    this.loadPlatforms = function () {
    };

    this.ajax = function (namespace, methodname) {
        return Promise.resolve(najax('http://www.google.com'));
    }

}

exports.Snowflake = Snowflake;