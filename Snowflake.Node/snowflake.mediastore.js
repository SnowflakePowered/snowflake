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
var MediaStore = function MediaStore(mediaStore, mediaStoreServer) {
    var enumerateImages = function (enumerateString) {
        from(this.Images).where(function (value, key) {
            return key.startsWith(enumerateString);
        }).toArray();
    }
        
    
    var buildAbsolute = function (mediaStoreSection, mediaStoreServer) {
        var _section = mediaStoreSection;
        from(mediaStoreSection).each(function (value, key) {
            console.log(key);
            console.log(value);
            _section[key] = mediaStoreServer + value;
        })
        return _section;
    }
    this.$ = mediaStore;
    this.Images = buildAbsolute(this.$.Images, mediaStoreServer);
    this.Fanarts = buildAbsolute(enumerateImages("fanart"));
    this.Screenshots = enumerateImages("screenshot");
	
	
	
}