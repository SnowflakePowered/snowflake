
![Snowflake](branding/Snowflake-Banner-Katakana-256.png) 
=========

**Build Status**

| <img src="https://cloud.githubusercontent.com/assets/1000503/13551021/797655f6-e2f9-11e5-8aea-a5caad2aeef8.png" width=25 alt="appveyor"> | <img src="https://cloud.githubusercontent.com/assets/1000503/13550984/6042f432-e2f8-11e5-95cf-72fb4134c56d.png" width=25 alt="travis"> | <img src="https://cloud.githubusercontent.com/assets/1000503/13551072/5f605ea8-e2fb-11e5-8641-d5efac977ead.png" width=25 alt="codecov"> | <img src="https://avatars1.githubusercontent.com/u/11671095?s=200&v=4" width=25 alt="codefactor"> | <img src="https://cloud.githubusercontent.com/assets/1000503/14840198/d3013102-0bff-11e6-945b-98d0728fb0b3.png" width=25 alt="license"> |
| ---------------------------------------- | ---------------------------------------- | ---------------------------------------- | ---------------------------------------- |----------------------------------------|
| [![Windows Build status](https://ci.appveyor.com/api/projects/status/mhei9fdtja5j04kk?svg=true)](https://ci.appveyor.com/project/RonnChyran/snowflake) | [![Travis](https://img.shields.io/travis/SnowflakePowered/snowflake.svg)](https://travis-ci.org/SnowflakePowered/snowflake) | [![Codecov](https://img.shields.io/codecov/c/github/SnowflakePowered/snowflake.svg)](https://codecov.io/gh/SnowflakePowered/snowflake) | [![CodeFactor](https://www.codefactor.io/repository/github/snowflakepowered/snowflake/badge)](https://www.codefactor.io/repository/github/snowflakepowered/snowflake) | [![License](https://img.shields.io/badge/license-mpl%202.0-blue.svg?style=flat)](https://github.com/SnowflakePowered/snowflake/blob/master/LICENSE) |

**Latest Version**

| <img src="https://cloud.githubusercontent.com/assets/1000503/13551043/3b0ac2f6-e2fa-11e5-886b-f6dfdc0ba6f9.png" width=25> | <img src="https://cloud.githubusercontent.com/assets/1000503/13551114/29c1f598-e2fd-11e5-8ad5-b2fa3a44e5ab.png" height=25> |
| ---------------------------------------- | ---------------------------------------- |
| [![MyGetFeed](https://img.shields.io/myget/snowflake-nightly/vpre/Snowflake.Framework.svg?style=flat)](https://www.myget.org/gallery/snowflake-nightly) | [![NuGet version](https://badge.fury.io/nu/Snowflake.Framework.svg)](https://www.nuget.org/packages/Snowflake.Framework) |


Snowflake is a framework for building flexible and beautiful emulator frontends. With features such as advanced configuration generation and an intelligent scraping system, Snowflake provides a powerful set of tools to manage, play, and organize your games when designing your dream frontend using HTML and modern Javascript.


Features
--------
Snowflake provides a comprehensive C# API and Javascript bindings to handle all aspects of an emulator frontend.

* Scraping
  * Efficient ROM file-type identification using filetype signatures.
  * [Stone](https://github.com/SnowflakePowered/stone)-mimetype based ROM types.
  * Plugin-based architecture for arbitrary information sources (TheGamesDB, OpenVGDB, etc.), and arbitrary media files (Automatic CUE generation, EmuMovies, Screenshots and Boxarts).
  * [Shiragame](https://github.com/SnowflakePowered/shiragame) hashed ROM database as a secondary identification source.
* Games
  * Multi-disc and multi-file oriented games database. 
  * Relational metadata and linked media files to each Game record.
  * Stone platform definitions for platform metadata and management.
* Emulators
  * On-the-fly per-game configuration generation and configuration metadata.
  * Comprehensive input management API for input configuration generation and metadata using Stone controller definitions.
  * Plugin-based architecture for emulation launch management allowing for increased control over the emulator launch process.
* Development
  * React-ready TypeScript bindings for all relevant APIs.
  * Electron-based desktop user interface.
  * Arbitrary binding support for additional bindings to any environment.
  * Fully documented C# API.
  * Full Linux support through .NET Core and .NET Standard.




Star our repository and bookmark our [website at http://snowflakepowe.red](http://snowflakepowe.red/) to keep up with Snowflake's development. We'll have something new in store for you soon!


Legal
-----
Snowflake is licensed under the Mozilla Public License 2.0 since Pull Request #231. Since May 1, 2016, this license is retroactively applied to all prior copies of Snowflake's source code. 

Snowflake does not in any way facilitate the download of illegal ROM images or warez of any kind. 
