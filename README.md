![Snowflake](branding/Snowflake-Banner-Katakana-256.png) 
=========
[![Build status](https://ci.appveyor.com/api/projects/status/mhei9fdtja5j04kk)](https://ci.appveyor.com/project/RonnChyran/snowflake)

Emulator API
- [ ] A way to store and load custom game profiles during configuration compilation (read-only)
- [ ]  A way to store and load configured controller profiles (filesystem?)
- [ ]  A way to store and load emulator-specific flags for certain games (use a database)
- [ ]  Implementing common logic in the abstract base class

Others
- [ ] A massive refactoring is required, sorting every class into a distinct API and creating and documenting interfaces for every core class
- [ ]  Completely remove the SharpYaml dependency, just migrate everything to json.net.
- [ ]  EventManager API should be implemented AFTER refactoring
- [ ]  Unit and integration tests if possible



Snowflake provides a plugin-based backend and a set of APIs powering an modern HTML5 frontend for Emulators with unprecendented integration and easy of use, without needing to modify or recompile the base emulator; allowing for maximum compatibility.


Documentation
-------------
Hang on while we get our API fully fleshed out! A massive refactor is planned separating interfaces from implementation, and the API itself is in a state of constant flux, additions and ABI breakages, we don't even have a version number yet!

Legal
-----
Snowflake is licensed under the Apache License 2.0. However any fork of the [codebase prior to the relicensing to Apache 2](https://github.com/snowflake-frontend/snowflake/commit/b0286553ec0887ce406420827a2ba0c20aa78117#diff-d41d8cd98f00b204e9800998ecf8427e) must be distributed under the GNU GPL v3. Any fork of the codebase after that commit is can be distributed under the Apache 2 license. Snowflake does not in any way facilitate the download of illegal ROM images or warez of any kind. 
