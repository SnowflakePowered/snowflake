# Architecture

This repository hosts the components required to bootstrap and run the core Snowflake server ("Snowflake"). It **does not** include any themes or UI components. It also includes the tooling, dependencies, and buildscripts required to build and extend Snowflake ("SDK") via its modular extension system.

At a high level, Snowflake is a web server that exposes a GraphQL API. This GraphQL API can be extended, and it exposes Snowflake's core functions and API that can be used to build an emulator frontend in combination with a UI.

Because of required source generators, the minimum supported .NET TFM (MSNV) is `net6.0`. Additionally, Visual Basic .NET is not supported.

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

Provides access to [Stone](https://stone.snowflakepowe.red/#/) platform and controller definitions. Also provides access to a variety of ROM file signatures that can help determine if a file can be assessed as a ROM with a valid Stone platform mimetype.

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

## Framework API

### Snowflake.Configuration

Implements emulator configuration representation and serialization. Configuration is organized in top-level `ConfigurationCollection` **template interfaces**, which consist of `ConfigurationSection` properties, which define `ConfigurationOption` properties. Source generators then generate required proxies from these template interfaces.

A `ConfigurationCollection` may represent one or more configuration files on disk, this can be specified via any number of `ConfigurationTarget` attributes. Values set in a `ConfigurationSection` are uniquely identified and saved to the store. A collection is rehydrated by its unique collection of values. 

Input configuration works similarly to a singlular `ConfigurationSection` template interface, but also allows `InputOption` properties that define a `DeviceCapability` value.

On serialization, a `ConfigurationCollection` is first traversed into a `AbstractConfigurationNode` AST by a `ConfigurationTraversalContext`, grouped by top-level root targets. `ConfigurationTarget` attributes form a DAG that is used to induce a nested structure over flatter structure of a `ConfigurationCollection`, since `ConfigurationSection` templates can not be nested. The structure of the AST will reflect the structure of the targets defined.

Once an AST has been produced, it can be further visited by a `ConfigurationTreeVisitor` to modify the AST. AST nodes can have `NodeAnnotations` attached to them to assist later tree visitor passes. Finally they are serialized to disk by an implementation of `ConfigurationSerializer`.

### Snowflake.Extensibility

Base classes and interfaces for `IPlugin` implementations. A `ProvisionedPlugin` has access to a directory scratch space where it can store configuration files and other auxillary files. The provision can either be determined via a `plugin.json` file, or opted out of via `StandalonePluginProvision` that does not require a `plugin.json` resource.

Plugin classes must be exported via the `Plugin` attribute, then manually registered with the `IPluginManager` during composition. Generally one module will register multiple plugins.

Plugin configuration is based off Snowflake.Configuration as a single `ConfigurationSection` template interface, although it is never serialized.

### Snowflake.Filesystem

Implements a virtual filesystem with an object-oriented filesystem API. This virtual filesystem is used extensively throughout Snowflake and is used to ensure consistent conventions and isolation throughout game and plugin folders. 

Directory capability permissions are encoded into the type system. Directories must be 'reopened' in order to be modified or deleted in specific ways. Symbolic links are transparent and treated as first-class citizens.

Files are transparently assigned unique GUID identifiers when opened within the context of a directory. This identifier is written as an extended attribute or alternate stream via [tsuku](https://github.com/SnowflakePowered/tsuku), and can be used to attach metadata within the context of a game by registering it as a `FileRecord`.

### Snowflake.Input

Abstractions for input configuration and devices. A distinction is made between a "virtual" configured gamepad that represents the controller-in-emulation ("Controller") and the physical input device used by the player to send inputs to the operating system ("Device").

*Snowflake.Input.Controller* implements the [Stone Controllers specification](https://stone.snowflakepowe.red/#/spec/controllers), and is mainly concerned about the mapping between real device inputs and the defined Controller layout input. A `ControllerElement` here refers to a virtual controller element defined by the Stone specification, and not any real device input.

*Snowflake.Input.Device* implements enumerators and definitions for the real input device. A device can have multiple instances when exposed to an operating system depending on the input APIs used, and each instance may have different properties, such as enumeration order. A `DeviceCapability` here refers to **the real capability** of a real input device, according to the input driver that enumerates the device. These capabilities are grouped together in `DeviceCapabilityClass` for ease of use.

An important difference here: a `ControllerMapping` is an "input profile" that specifies the **realized** mappings between the `DeviceCapability` of a real device, and the emulated `ControllerElement`, and is **user configurable**. A `DeviceLayoutMapping` is the **idealized inverse**, and is **not user configurable**: given a `ControllerElement`, what is the default or canonical mapping from the `ControllerElement` to the device I have in my hand? For example, the A button on a **real Xbox One controller**, which is `DeviceCapability.Button0` when enumerated via XInput, would map to `ControllerElement.ButtonA` for the `NES_CONTROLLER` layout.

### Snowflake.Installation
Implements the declarative game installation API. Given a platform and a list of directories and files, `IGameInstaller` will return what it knows is processable, and then later process the installation if the user wishes. Installtion is presented by yielding an asynchronous series of predefined tasks. 

Third parties may also implement additional atomic asynchronous tasks. Each task represents a step in the installation process, which can be paused, resumed, and cancelled at a later time. Tasks **are not allowed to throw**, and return a monadic `TaskResult` wrapper over the result of the IO task that can be `await`-ed on to bind and 'unwrap' the inner result. Awaiting the same `TaskResult` multiple times will return the same cached result. Tasks also take as input `TaskResult` wrappers to ensure that IO operations are lazy and efficient. 

### Snowflake.Loader

Implements the module loader and enumerator. Mostly JSON parsing routines for parsing and validating `module.json`. This namespace also includes interfaces for implementing custom `IModuleLoader` instances, and the `ServiceProvider` which handles importing of services during composition.

#### Snowflake.Services.AssemblyLoader

The loading of assembly modules is implemented in the namespace Snowflake.Services.AssemblyLoader.

Loading modules into Snowflake is a three-step process.

1. An `AssemblyLoadContext` attempts to load the `entryPoint` DLL assembly specified in a `module.json`, including dependent assemblies from the currently loaded context (i.e. Snowflake framework assemblies and the BCL), dependent assemblies in the local directory where the module is located, and the GAC as well, trying its best to resolve different versions following semver. It also handles caching of unsigned assemblies, which is necessary, otherwise they will be reloaded into the context multiple times. 
2. Once the assemblies are loaded into a context, `AssemblyModuleLoader` discovers classes that implement `IComposable` with a nilladic default constructor.
3. From the discovered composables, a simple dependency resolution load now takes place. Composables that rely on a service (which are most non-trivial composables) must specify their service dependencies via the `[ImportService]` attribute. The `AssemblyComposer` takes the list of discovered composables and composes all that have their dependencies fulfilled, which may register more services during composition. This repeats until no more composables can be composed. This means that composables that are missing dependencies will never be composed.

### Snowflake.Model

Contains the implementation for all SQLite databases used to store data, including games, file records, and emulator configuration settings. 

While in the backend the database is implemented via code-first EntityFramework Core, the EF interface (`DbContext`) is never exposed to API consumers. Instead, the repository pattern is implemented as an interface over the EF backend.

For games and files in particular, metadata may be attached. Such items that can have metadata attached are referred to as 'Records'. The `IGameLibrary` API is also extensible to retrieve data from sources other than the main database (which is not extensible by third parties) via crossreference of the game's `RecordID`. This is how the file record API is implemented. 

Because of its extensibility, the `IGameRecord` interface is infinitely flexible and not well suited for EntityFramework rewriting in order to perform performant queries. A restricted subset, `IGameRecordQuery` is accepted for use with the `IGameLibrary.QueryGames` APIs, which are directly executed on the database and are much more performant. For complex use cases, `IGameLibrary.GetGames` is still available and performs client-side filtering.

File records are simply files that have their unique IDs registered as 'belonging' to a game, with **a mimetype** and some metadata attached. Not all files need to be records, only those that are significant to the execution of the game itself. For most games, the ROM or ISO file containing the game data would be a record, as well as some boxarts and images. In particular, the **entry point** of a game must be a record in order for the game to be executable. For some this would be the ROM, for others, an EBOOT or an ELF.

### Snowflake.Orchestration

Provides support for managing an external emulated instance. Base class implementations for `EmulatorOrchestrator` plugins, save management, process management, and directory projections for a running instance of an emulator.

An `EmulatorOrchestrator` is essentially a factory for a `GameEmulation` instance that manages the lifecycle of a running emulator instance. The `GameEmulation` is responsible for setting up the environment by restoring save games from the managed save directory, setting up symbolic links via directory projections to mirror the expected structure, and copying necessary BIOS files, as well as teardown once the emulation stops: persisting savegame data, cleaning up symbolic links, making sure things are where they are supposed to be. 

### Snowflake.Romfile

Provides APIs having to do with ROM file analysis, such as file signatures, and best-effort ROM filename parsing. The filename parser is currently regex-based and is due to be replaced with a port of the parser from [shiratsu](https://github.com/SnowflakePowered/shiratsu/tree/master/src).

### Snowflake.Scraping

Snowflake uses an iterative tree-based, best effort scraping system that is unique across all other frontends. A `Game` is first created on the database for a known platform and perhaps a ROM registered as a file record. A `GameScrapeContext` is then created when a scrape is requested. The context contains the scrapers that will produce 'seeds', and a culler that removes seeds that seem unlikely to contribute to relevant data.

Seeds are tuples of the form `(type, string)`, and are contained in a tree of the form `(Seed, Seed[])`, which are yielded by scrapers or seeded by the scrape context with information from the game at the time of context creation. A `Scraper` looks at a parent seed, as well as any seeds at the sibling, child, or root level relative to the parent seed in order to make decisions about scraping. It then yields whatever results in tree form, which are then attached as a child or sibling of the parent seed, or as a child of the root seed. 

Scrapers can specify dependencies using directives, which allow them to execute only when a seed of a specific type is attached at a specific position, either as part of the root, as a sibling, or as parent, although a parent dependency is already implied. Scrapers can also refuse to run if a certain directive is fulfilled, ensuring that irrelevant scrapers will not be unnecessarily ran.

The scraping process is iterative and keeps going until no new seeds are added. After no more seeds can be added, cullers are run to pick the best result compared to a ground truth. The default culler does this by comparing the result `title` seed with the search query title via their Jaccard coefficient.

This is what is meant by iterative tree-based scraping. This approach is extremely powerful. For example, a game scrape context starts out with some hashes, which a shiragame scraper would emit a `search_title` seed. Then, another scraper that for example calls GiantBomb would emit a `result` seed with a bunch of information using the `search_title` as a basis. 

Once the scrape and cull cycle is done, there should remain a single seed of each type, usually under `result`. Traversers then walk this seed tree and cause side effects, such as writing metadata to the game or downloading boxart URLs.