# Architecture

This repository hosts the components required to bootstrap and run the core Snowflake server ("Snowflake"). It **does not** include any themes or UI components. It also includes the tooling, dependencies, and buildscripts required to build and extend Snowflake ("SDK") via its modular extension system.

At a high level, Snowflake is a web server that exposes a GraphQL API. This GraphQL API can be extended, and it exposes Snowflake's core functions and API that can be used to build an emulator frontend in combination with a UI.

Because of required source generators, the minimum supported .NET TFM (MSNV) is `net5.0`. Additionally, Visual Basic .NET is not supported.

## Plugins vs. Modules

A distinction is made between extension "Modules" and "Plugins". A module is loaded by the module loader, and need not be a .NET assembly. Plugins are specifically C# classes that implement `Snowflake.Extensibility.IPlugin`, and are typically serve a fixed purpose, such as `Scraper`, `GameInstaller`, or `EmulatorOrchestrator`. 

An 'assembly' module *may* register plugins through the `IPluginManager`, but this is not necessary. Assembly modules can run arbitrary code during their composition entry point `Compose`, and can also register additional service singletons via `IServiceRegistrationProvider` that are not limited to instances of Plugins.

Modules are defined by their loader. By default, Snowflake registers a single loader, `assembly` which loads .NET assemblies and calls the `IComposable.Compose` entrypoint on implementing classes. Additional loaders can be implemented via the `Snowflake.Loader.IModuleLoader` interface.

## Projects

Projects in this repository are either "Framework" projects that consist of Snowflake's core API, including it's frontend-focused APIs as well as internal APIs such as those required to load and host plugin and modules, or "Support" projects which provide data and APIs used to host a runtime instance of Snowflake. Framework projects are considered part of the SDK and must be referenced by extensions, but Support projects are extensions themselves that are required to start a usable instance of Snowflake.

Also included are "Bootstrap" projects that simply act as an entry point into the web server and instantiates the module loader, etc. "Plugin" projects are currently only in the repository for convenience purposes, to ensure API compatibility while the SDK is moving before 1.0. Plugins will be moved over to the "SnowflakeContrib" root level namespace before the 1.0 API finalization.

### Snowflake.Framework

This project contains the implementations of the majority of Snowflake's APIs. It also exposes the base implementations of plugin extension types.

### Snowflake.Framework.Primitives

This project contains the interface definitions and some basic data structs for all APIs. Typically, every interface has an implementation in either Snowflake.Framework, or elsewhere. This one-interface-per-class approach is useful for testing and reusing Snowflake's base types in projects that may not support the minimum supported .NET version.

### Snowflake.Framework.Services

This project contains the base services required to bootstrap the Snowflake server. This includes the service container, assembly module loader (`AssemblyModuleLoader`), ASP.NET Core server (`KestrelServerService`), base directory (`ContentDirectoryProvider`), and others. 

Note that the Snowflake service container **is not the ASP.NET Core `IServiceProvider`**. Outside of Kestrel middleware (accessible via `Snowflake.Remoting.Kestrel.IKestrelServerMiddlewareProvider`) and GraphQL schemas (via `Snowflake.Remoting.GraphQL.IGraphQLSchemaRegistrationProvider`), ASP.NET Core services are **not accessible**. 

### Snowflake.Framework.Language

Source generators and analyzers for Snowflake extensions. The framework implementation, plugins, and configuration generation relies on these source generators to compile properly. Also includes analyzers to ensure correct usage of configuration types and module composition.

### Snowflake.Framework.Remoting

Interfaces and base class implementations for web servers and remoting. This is taken out from Snowflake.Framework.Primitives to keep the base library transport agnostic, since Snowflake.Framework.Remoting takes a dependency on ASP.NET Core.

### Snowflake.Framework.Remoting.GraphQL

Interfaces and base classes for GraphQL extensions, as well as the GraphQL types to model Snowflake's base types.

### Snowflake.Support.InputEnumerators.Windows

Implements an input enumerator for Windows using DirectInput and XInput.

### Snowflake.Support.StoneProvider

Provides access to [Stone](https://stone.snowflakepowe.red/#/) platform and controller definitions.

### Snowflake.Support.StoreProviders

Sets up and provides access to databases, including the game library, file record library, configuration and input settings database, etc., as well as setting up default game library extensions such as game files.


### Snowflake.Support.Orchestration.Providers

Support servcies for emulators (orchestration). Registers a module loader for packaged emulator executables.

### Snowflake.Support.Scraping.Primitives

Default implementations of cullers and metadata traversers for scrapers.

### Snowflake.Support.Remoting.Electron.ThemeProvider

Support services for hosted Electron themes. Registers a module loader for ASAR package types, as well as a GraphQL extension to query available themes.

### Snowflake.Framework.Library

SDK project that defines the base dependencies for extensions that consume Snowflake. Automatically referenced when using Snowflake's SDK. 

### Snowflake.Framework.Sdk

SDK props and targets for extensions that consume Snowflake. 

### Snowflake.Framework.Dependencies

SDK project that defines a base set of common dependencies in addition to the base class library of the MSNV. Should not be directly referenced.

### Snowflake.Framework.Dependencies.Sdk

SDK props and targets for Snowflake.Framework.* projects. Only used internally, and should not be directly referenced.

### Snowflake.Framework.Tests

Platform-agnostic xUnit tests for all Snowflake projects, including support projects.

### Snowflake.Tooling.Taskrunner

CLI used to package and install Snowflake modules.

### Snowflake.Templates.AssemblyModule

Assembly module project template. 

## API

### Snowflake.Configuration

Implements emulator configuration representation and serialization. Configuration is organized in top-level `ConfigurationCollection` **template interfaces**, which consist of `ConfigurationSection` properties, which define `ConfigurationOption` properties. Source generators then generate required proxies from these template interfaces.

A `ConfigurationCollection` may represent one or more configuration files on disk, this can be specified via any number of `ConfigurationTarget` attributes. Values set in a `ConfigurationSection` are uniquely identified and saved to the store. A collection is rehydrated by its unique collection of values. 

Input configuration works similarly to a singlular `ConfigurationSection` template interface, but also allows `InputOption` properties that define a `DeviceCapability` value.

On serialization, a `ConfigurationCollection` is first traversed into a `AbstractConfigurationNode` AST by a `ConfigurationTraversalContext`, grouped by top-level root targets. `ConfigurationTarget` attributes form a DAG that is used to induce a nested structure over flatter structure of a `ConfigurationCollection`, since `ConfigurationSection` templates can not be nested. The structure of the AST will reflect the structure of the targets defined.

Once an AST has been produced, it can be further visited by a `ConfigurationTreeVisitor` to modify the AST. AST nodes can have `NodeAnnotations` attached to them to assist later tree visitor passes. Finally they are serialized to disk by an implementation of `ConfigurationSerializer`.

