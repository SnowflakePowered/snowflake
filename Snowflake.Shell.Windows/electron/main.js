var app = require('app');  // Module to control application life.
var BrowserWindow = require('browser-window');  // Module to create native browser window.

// Report crashes to our server.
require('crash-reporter').start();

// Keep a global reference of the window object, if you don't, the window will
// be closed automatically when the javascript object is GCed.
var mainWindow = null;
var firstRun = false;
// Quit when all windows are closed.
app.on('window-all-closed', function() {
  if (process.platform != 'darwin') {
    app.quit();
  }
});

if(process.argv.indexOf("--firstrun") > 0){
  firstRun = true;
}
global.packageManagerUrl = "http://localhost:33333";

// This method will be called when Electron has done everything
// initialization and ready for creating browser windows.
app.on('ready', function() {
  // Create the browser window.
  mainWindow = new BrowserWindow({'node-integration' : true, 'width': 800, 'height': firstRun ? 500 : 600, 'frame': true, resizable: !firstRun, icon: 'icon.png', center: firstRun});

  // and load the index.html of the app.
  if(firstRun){
  //  mainWindow.loadUrl('file://' + __dirname + '/_firstrun_index.html');
  }else{
    mainWindow.loadUrl('file://' + __dirname + '/_test.html');
  }
  // Open the devtools.
  //mainWindow.openDevTools();

  // Emitted when the window is closed.
  mainWindow.on('closed', function() {
    // Dereference the window object, usually you would store windows
    // in an array if your app supports multi windows, this is the time
    // when you should delete the corresponding element.
    mainWindow = null;
  });
});
