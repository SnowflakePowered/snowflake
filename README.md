Snowflake
=========
[![Build status](https://ci.appveyor.com/api/projects/status/mhei9fdtja5j04kk)](https://ci.appveyor.com/project/ron975/snowflake)


Snowflake is an emulator frontend for Windows that emphasizes ease-of-use, extensibility and aesthetics. Snowflake provides an extensive plugin API, and HTML5 UI system that allows modern web-design techniques to theme the frontend. Snowflake is currently in pre-alpha. 

Plugins
-------
Snowflake considers plugins to be a vital part of the frontend. 
Plugins may be of one of the following types:
  * Emulators
  * Scrapers
  * Identifiers
  * General Plugins
  
### Emulators
An Emulator plugin is similar to an OpenEmu core, in the way they allow the frontend access to an emulator. However, unlike OpenEmu cores, which _require_ all emulator logic to be part of the plugin, Snowflake's Emulator plugins are extremely flexible. All an emulator plugin must do is provide an endpoint to launch an emulator of any type, given a filename, and the emulated platform. 

Thus, an Emulator plugin can be as simple as wrapping the RetroArch executable and calling it with the proper arguments, or as complex as calling libretro directly. This allows for not only OpenEmu style, libretro integration, but also wrapping executable emulators such as PCSX2 or Dolphin, emulators that do not yet have a libretro implementation. 

APLHA: Currently, only a basic RetroArch executable wrapper plugin has been implemented. 

### Scrapers
Deriving inspiration from popular HTPC applications, Snowflake provides a easy to use API to write game info scrapers from. Scrapers allow Snowflake to provide complete information for a game, given a search query. Utilizing popular game information databases, scrapers remove most, if not all the work in finding relevant metadata for each game.

### Identifiers
Snowflake is designed to be as noob-friendly as possible, welcoming newcomers to the emulation scene. Certainly there are a large portion of emulation newcomers, or even enthusiasts that do not maintain their ROM collection with a standard naming convention. Identifiers solve this problem through similar methods as popular tools such as ClrMamePro. They are to identify a ROM through all means possible before passing this information onto the scraper. This includes digging through ROM or ISO file headers to find a unique identifier. 

ALPHA: Currently, the only identifier written can identify ROMs through CRC data in TOSEC and No-Intro dats. Identifiers that grab information through file headers will soon be implemented.

### General Plugins
Snowflake will provide an extensive event API that will allow plugins to control nearly every aspect of the frontend. With maturity, Snowflake's plugin system will allow endless possibilities.

Interface
---------
Snowflake's interface is powered by node-webkit. Events are delegated to the frontend through an abstraction of window.postMessage, and Snowflake provides a robust JSONP API through GET requests that is easily accessible through the web browser.

Snowflake provides a GET API to interact with the core. The Information API, for example to get information regarding a Game or a Platform, and will return a reply. The Command API directs Snowflake to _do_ something, such as adding a game to the database, running a game, or notifying Snowflake of UI events, return no useful reply, besides an acknowledgement that the command has been received. Such commands may result in an event delivered to the UI, through either JSONRPC if using node-webkit, or direct method call using CefSharp. The API that receives events is abstracted from the implementation, regardless of which UI is used, ensuring compatibility with all skins.

### Materia
Materia is Snowflake's default theme, written in HTML5 using the Polymer framework. Themes do not need to use Polymer framework, and only need to respond to the Javascript window message APIs.

API Documentation
-----------------
This early in development, the API specification is still uncertain. Further along, more comprehensive API documentation will be provided.\

Legal
-----
Snowflake is licensed under the Apache License 2.0. Snowflake does not in any way facilitate the download of illegal ROM images or warez of any kind. 
