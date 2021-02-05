
![Snowflake](branding/horizon/snowflake/exports/Logo-Logotype@500px.png) 
=========

**Build Status**

| Windows | Linux | <img src="https://cloud.githubusercontent.com/assets/1000503/13551072/5f605ea8-e2fb-11e5-8641-d5efac977ead.png" width=25 alt="codecov"> | <img src="https://avatars1.githubusercontent.com/u/11671095?s=200&v=4" width=25 alt="codefactor"> | <img src="https://avatars0.githubusercontent.com/ml/1714" width=25 alt="lgtm">| <img src="https://cloud.githubusercontent.com/assets/1000503/14840198/d3013102-0bff-11e6-945b-98d0728fb0b3.png" width=25 alt="license"> |
| ---------------------------------------- | ---------------------------------------- | ---------------------------------------- | ---------------------------------------- |---------------------------------------- | -------------------------------------------------- | 
| [![Windows Build status](https://dev.azure.com/SnowflakePowered/snowflake/_apis/build/status/SnowflakePowered.snowflake-win?branchName=master)](https://dev.azure.com/SnowflakePowered/snowflake/_build?definitionId=1) | [![Linux](https://dev.azure.com/SnowflakePowered/snowflake/_apis/build/status/SnowflakePowered.snowflake-linux?branchName=master)](https://dev.azure.com/SnowflakePowered/snowflake/_build?definitionId=2) | [![Codecov](https://img.shields.io/codecov/c/github/SnowflakePowered/snowflake.svg)](https://codecov.io/gh/SnowflakePowered/snowflake) | [![CodeFactor](https://www.codefactor.io/repository/github/snowflakepowered/snowflake/badge)](https://www.codefactor.io/repository/github/snowflakepowered/snowflake) | [![Total alerts](https://img.shields.io/lgtm/alerts/g/SnowflakePowered/snowflake.svg?logo=lgtm&logoWidth=18)](https://lgtm.com/projects/g/SnowflakePowered/snowflake/alerts/) | [![License](https://img.shields.io/badge/license-mpl%202.0-blue.svg?style=flat)](https://github.com/SnowflakePowered/snowflake/blob/master/LICENSE) |


**Latest Version**

| <img src="https://cloud.githubusercontent.com/assets/1000503/13551043/3b0ac2f6-e2fa-11e5-886b-f6dfdc0ba6f9.png" width=25> | <img src="https://cloud.githubusercontent.com/assets/1000503/13551114/29c1f598-e2fd-11e5-8ad5-b2fa3a44e5ab.png" height=25> |
| ---------------------------------------- | ---------------------------------------- |
| [![MyGetFeed](https://img.shields.io/myget/snowflake-nightly/vpre/Snowflake.Framework.svg?style=flat)](https://www.myget.org/gallery/snowflake-nightly) | [![NuGet version](https://badge.fury.io/nu/Snowflake.Framework.svg)](https://www.nuget.org/packages/Snowflake.Framework) |


Snowflake is a framework for building flexible and beautiful emulator frontends. With features such as advanced configuration generation and an intelligent scraping system, Snowflake provides a powerful set of tools to manage, play, and organize your games when designing your dream frontend using the language of your choice.


Features
--------
Snowflake features innovative solutions to many problems with current emulator frontends.
* Full cross-patform support through .NET Core.
* Dynamic and flexible tree-based game scraping.
* Programmatic per-game emulator configuration generation and input management.
* Multi-disc and multi-file capable relational games database.

At its core, Snowflake is designed to be moddable and easy to develop for, with a comprehensive C# API for extensibility and GraphQL interface for frontend UIs.
* Language-agnostic GraphQL interface for communicating with the Snowflake framework.
* Plugin API to extend the framework with C# plugins.
* Module-based runtime extensibility.
* [Stone](https://github.com/SnowflakePowered/stone) platform, controller, and canonical ROM file mimetype compliant.
* Dedicated CLI to help you get started quickly and easily. 

Star this repository and bookmark our [website at http://snowflakepowe.red](http://snowflakepowe.red/) to keep up with Snowflake's development. We'll have something new in store for you soon!

Getting Started
---------------

Snowflake does not currently have a well defined install process for end-users, but it is easy to set up a development environment. 
You will need the [.NET 5 SDK](https://www.microsoft.com/net/download/) to get started. 

```cli
$ git clone --recursive https://github.com/SnowflakePowered/snowflake/
$ cd snowflake
$ dotnet run --project build -- Bootstrap
```

This command will build and install all support modules required for Snowflake to be functional to your application data directory (`%appdata%\snowflake\modules` or `~/.snowflake/snowflake/modules` on Linux). Note that you may have to delete the installed module `assembly.Snowflake.Framework.Test.InvalidComposable`, which is designed deliberately to error on load, however this is not necessary.

As well, input management APIs are currently only available on Windows. Attempting to use these APIs on Linux will fail to enumerate any device.

Legal
-----
Snowflake is licensed under the Mozilla Public License 2.0 (MPL2). Since May 1, 2016, this license is retroactively applied to all prior copies of Snowflake's source code which may have been licensed under different terms. 

Snowflake does not in any way facilitate the download of illegal ROM images or warez of any kind. 
