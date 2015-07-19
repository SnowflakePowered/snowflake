var $ = require("jQuery")
var exports = module.exports = {};


exports.queue = [];
exports.lock = false;
exports.counter = 0;
exports.downloadFile = function(url, description){
  var queuedDownload = function() {
      $('.download-progress-status').each(function(index){
        $(this).html("Updating " + description);
      });
      window.setTimeout(function(){
        fs.mkdir((process.env.APPDATA || (process.platform == 'darwin' ? process.env.HOME + 'Library/Preference' : '/var/local')) + "/Snowflake/staging/", function(err){});
        var fullpath = (process.env.APPDATA || (process.platform == 'darwin' ? process.env.HOME + 'Library/Preference' : '/var/local')) + "/Snowflake/staging/" +
         (url ? url.split('/').pop().split('#').shift().split('?').shift() : null);
        description = description || (url ? url.split('/').pop().split('#').shift().split('?').shift() : null);
        exports.lock = true;

        $.ajax({
          url: url,
          success: function(data){
            console.log("success");
            fs.writeFile(fullpath, data, function(err){
              if (err) {
                $('.download-progress-bar').each(function(index){
                  $(this).addClass("progress-bar-danger");
                });
                $('.download-progress-status').each(function(index){
                  $(this).html("Error downloading " + description);
                });
              } else {
                console.log("Saved to " + fullpath);
                $('.download-progress-bar').each(function(index){
                  $(this).css({width :"100%"});
                });
                $('.download-progress-bar-label').each(function(index){
                  $(this).html("100%");
                });
                $('.download-progress-status').each(function(index){
                  $(this).html("Updated " + description);
                });
              }
              exports.queue.shift();
              delete exports.current;
              window.setTimeout(function(){
                exports.lock = false;
              }, 1000);
              window.dispatchEvent(new Event('downloadcomplete'));
            });
          },
          cache: false,
          error: function (xhr, ajaxOptions, thrownError) {
              console.log(xhr.responseText);
              console.log(thrownError);
              $('.download-progress-status').each(function(index){
                $(this).html("Error downloading " + description);
              });
              $('.download-progress-bar').each(function(index){
                $(this).addClass("progress-bar-danger");
              });
              console.warn("Could not download file " + url);
              exports.queue.shift();
              delete exports.current;
              window.setTimeout(function(){
                exports.lock = false;
              }, 1000);
              window.dispatchEvent(new Event('downloaderror'));
          },
          xhr: function() {
             var xhr = new window.XMLHttpRequest();
             xhr.addEventListener("progress", function(event){
               if (event.lengthComputable) {
                 var percentComplete = event.loaded / event.total;
                 $('.download-progress-bar').each(function(index){
                   $(this).css({width : percentComplete * 100 + "%"});
                 });
                 $('.download-progress-bar-label').each(function(index){
                   $(this).html(Math.floor(percentComplete * 100) + "%");
                 });
               }
             }, false);
             return xhr;
           }
        });
      }, 1000);
    }
    exports.queue.push(queuedDownload);
}
exports.worker = window.setInterval(function(){
  if(!exports.lock && exports.queue[0] !== undefined && exports.current === undefined){
      $('.download-progress-bar').each(function(index){
        $(this).css({width : "0%"});
      });
      $('.download-progress-bar').each(function(index){
        $(this).removeClass("progress-bar-warning");
        $(this).removeClass("progress-bar-danger");
      });
      $('.download-progress-status').each(function(index){
        $(this).html("<br/>");
      });
      $('.download-progress-bar-label').each(function(index){
        $(this).html("0%");
      });
      exports.current = window.setTimeout(function(){
      exports.queue[0]();
    }, 2000);
  }
}, 0);
