Snowflake
=========

> As of this C# implementation Snowflake-csharp is no longer strictly an implementation of Icelake. Instead, Icelake and pySnowflake is now deprecated and all future Snowflake development will be under Snowflake-Csharp. As a result, compatibility under non-Windows systems is no longer guaranteed but development will continue with *nix compatibility in mind for the future

What is Snowflake?
------------------
Snowflake, on the surface, aims to be [http://openemu.org/](OpenEmu) for Windows. But a pretty interface is only a part of Snowflake's goal. Snowflake is designed to be as modular as possible, providing a robust event-based plugin architecture through the Managed Extensibility Framework. Snowflake will support external, executable emulators such as Dolphin and PCSX2, and in the future, through P/Invoke or C++/CLI, libRetro cores as well. 

A powerful and flexible API will be provided to General Plugins that can access Snowflake's events. Specialized APIs for Scrapers and Emulators ensure easy integration with existing emulators and services. Snowflake aims to have full scraping support, all one would need to do is add the ROM, and press play. 

Customizability is core to Snowflake's design. The User Interface is written using the latest HTML5 and Javascript technology, rendered by Chromium/Blink through CEFSharp. As CEFSharp is merely acting as the renderer, both CEFSharp and the HTML+JS UI can be switched out. As the underlying HTML is able to be edited, Snowflake will have a powerful theming framework. 

Currently, Snowflake is largely incomplete. The API will be very volatile until the first Major release. Snowflake follows the Semantic Versioning syntax; leaving the build number as 0.

Building
--------
Visual Studio 2013 is required to build this project. No PluginManagers have been implemented, thus functionality is next to none at the moment. 
