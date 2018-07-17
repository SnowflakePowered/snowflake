
![Snowflake](branding/horizon/snowflake/exports/Logo-Logotype@500px.png) 
=========

**Build Status**

| <img src="https://cloud.githubusercontent.com/assets/1000503/13551021/797655f6-e2f9-11e5-8aea-a5caad2aeef8.png" width=25 alt="appveyor"> | <img src="https://cloud.githubusercontent.com/assets/1000503/13550984/6042f432-e2f8-11e5-95cf-72fb4134c56d.png" width=25 alt="travis"> | <img src="https://cloud.githubusercontent.com/assets/1000503/13551072/5f605ea8-e2fb-11e5-8641-d5efac977ead.png" width=25 alt="codecov"> | <img src="https://avatars1.githubusercontent.com/u/11671095?s=200&v=4" width=25 alt="codefactor"> | <img src="https://cloud.githubusercontent.com/assets/1000503/14840198/d3013102-0bff-11e6-945b-98d0728fb0b3.png" width=25 alt="license"> |
| ---------------------------------------- | ---------------------------------------- | ---------------------------------------- | ---------------------------------------- |----------------------------------------|
| [![Windows Build status](https://ci.appveyor.com/api/projects/status/mhei9fdtja5j04kk?svg=true)](https://ci.appveyor.com/project/RonnChyran/snowflake) | [![Travis](https://img.shields.io/travis/SnowflakePowered/snowflake.svg)](https://travis-ci.org/SnowflakePowered/snowflake) | [![Codecov](https://img.shields.io/codecov/c/github/SnowflakePowered/snowflake.svg)](https://codecov.io/gh/SnowflakePowered/snowflake) | [![CodeFactor](https://www.codefactor.io/repository/github/snowflakepowered/snowflake/badge)](https://www.codefactor.io/repository/github/snowflakepowered/snowflake) | [![License](https://img.shields.io/badge/license-mpl%202.0-blue.svg?style=flat)](https://github.com/SnowflakePowered/snowflake/blob/master/LICENSE) |

**Latest Version**

| <img src="https://cloud.githubusercontent.com/assets/1000503/13551043/3b0ac2f6-e2fa-11e5-886b-f6dfdc0ba6f9.png" width=25> | <img src="https://cloud.githubusercontent.com/assets/1000503/13551114/29c1f598-e2fd-11e5-8ad5-b2fa3a44e5ab.png" height=25> |
| ---------------------------------------- | ---------------------------------------- |
| [![MyGetFeed](https://img.shields.io/myget/snowflake-nightly/vpre/Snowflake.Framework.svg?style=flat)](https://www.myget.org/gallery/snowflake-nightly) | [![NuGet version](https://badge.fury.io/nu/Snowflake.Framework.svg)](https://www.nuget.org/packages/Snowflake.Framework) |


Snowflake is a framework for building adaptable and beautiful emulator frontends. With features such as advanced configuration generation and an intelligent scraping system, Snowflake provides a powerful set of tools to manage, play, and organize your games when designing your dream frontend (in the language of your choice).


Features
--------
Snowflake features innovative solutions to many problems with current emulator frontends.
* Full cross-patform support through .NET Core.
* Dynamic and flexible tree-based game scraping.
* Programmatic per-game emulator configuration generation and input management.
* Multi-disc and multi-file capable relational games database.

Snowflake is designed to be modifiable and easy to develop use, with a comprehensive C# API for extensibility and GraphQL interface for frontend UIs.
* Language-agnostic GraphQL interface for communicating with the Snowflake framework.
* Plugin API to extend the framework with C# plugins.
* Module-based runtime extensibility.
* [Stone](https://github.com/SnowflakePowered/stone) platform, controller, and canonical ROM file mimetype compliant.
* Dedicated CLI to help you get started quickly and easily. 

Star this repository and bookmark our [website at http://snowflakepowe.red](http://snowflakepowe.red/) to keep up with Snowflake's development. We will have something new in store for you soon!

Getting Started
---------------

Snowflake does not currently have a well defined install process for users, but it is easy to set up a development environment. 
You will need the [.NET Core SDK 2.1](https://www.microsoft.com/net/download/) to get started. 

**Windows**
```cli
> git clone --recursive https://github.com/SnowflakePowered/snowflake/
> cd snowflake
> cd build
> .\build.ps1 -Target Bootstrap
```

**Linux**
```cli
$ git clone --recursive https://github.com/SnowflakePowered/snowflake/
$ cd snowflake
$ cd build
$ ./build.sh -target=Bootstrap
```

This command will build and install all support modules required for Snowflake to be functional to your application data directory (`%appdata%\snowflake\modules` or `~/.snowflake/snowflake/modules` on Linux). Note that you may have to delete the installed module `assembly.Snowflake.Framework.Test.InvalidComposable`, which is designed deliberately to display an error on load. The deletion, however, is not necessary.

Input management APIs are currently only available on Windows. Attempting to use these APIs on Linux will fail to enumerate any device.

Legal
-----
Snowflake is licensed under the Mozilla Public License 2.0 (MPL2) with certain parts dual licensed under the GNU General Public License version 3 and MPL2 as indicated. Since May 1, 2016, this license is retroactively applied to all prior copies of Snowflake's source code which may have been licensed under different terms. 

Snowflake does not in any way facilitate the download of illegal ROM images or warez of any kind. 
