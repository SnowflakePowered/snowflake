var najax = require("najax");
var Promise = require('promise');
var from = require("fromjs");
if (!String.prototype.startsWith) {
    Object.defineProperty(String.prototype, 'startsWith', {
        enumerable: false,
        configurable: false,
        writable: false,
        value: function (searchString, position) {
            position = position || 0;
            return this.lastIndexOf(searchString, position) === position;
        }
    });
}
/* non-snowflake code end*/

var SnowflakeApi = function SnowflakeApi(snowflakeUrl) {
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

var MediaStore = function MediaStore(mediaStore) {
    var enumerateImages = function (enumerateString) {
        var from = require("fromjs");
        return from(instance.$.Images.MediaStoreItems).where(function (value, key) {
            console.log(key);
            return key.startsWith(enumerateString);
        }).toArray();
    }
    var instance = this;
    this.$ = mediaStore;
    this.Images = this.$.Images.MediaStoreItems;
    this.Video = this.$.Video.MediaStoreItems;
    this.Audio = this.$.Audio.MediaStoreItems;
    this.Resources = this.$.Resources.MediaStoreItems;
    this.Fanarts = enumerateImages("fanart");
    this.Screenshots = enumerateImages("screenshot");
}

exports.MediaStore = MediaStore;
exports.SnowflakeApi = SnowflakeApi;
