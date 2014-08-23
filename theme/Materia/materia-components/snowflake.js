var najax = require("najax");
var Promise = require('promise');
var Snowflake = function Snowflake(snowflakeUrl) {
    var instance = this;

    this.snowflakeUrl = snowflakeUrl;
    this.platforms = {};
    this.games = {};
    this.loadGames = function () {
        return this.ajax('Core', 'Game.GetAllGames').then(function (response) {
            instance.games = JSON.parse(response);
        }).catch(function (fail) {
            console.log(fail);
        });
    };
    this.loadPlatforms = function () {
        return this.ajax('Core', 'Platform.GetAllPlatforms').then(function (response) {
            instance.platforms = JSON.parse(response);
        }).catch(function (fail) {
            console.log(fail);
        });
    };

    this.ajax = function (namespace, methodname) {
        return Promise.resolve(najax(snowflakeUrl + '/' + namespace + '/' + methodname));
    }
}

exports.Snowflake = Snowflake;
