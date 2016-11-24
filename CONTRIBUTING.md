Contributing to Snowflake
=========================

Commits
-------
Snowflake recommends that each commit affects mostly one namespace at a time. Commit messages should be prepended with a short identifier on what part of Snowflake was affected, followed by a colon, for example, `Service.PluginManager: Fix plugin loading`, `Build-Appveyor: Fix Appveyor Builds`, or `Tests: Add Controller Tests`. Long descriptions are not required but can be put under the short, one-liner description. If your change breaks the existing API by removing classes or interfaces, changing method signatures or otherwise require plugins to recompile, prepend your commit message with `BREAKING` in upper case. Breaking changes may be scrutinized heavily and may not be merged after Snowflake releases. However currently the API is still in it's infancy, and many breaking changes are to be expected.

Pull Requests
-------------
Snowflake suggests you work on one feature per branch, please do so to maintain clean pull request records. When you are ready to file a pull request, please follow these guidelines. If you are a new contributor, you must append your name and contact information to the NOTICE file at the root of the repository. Please see the Legal section below for more details.

Snowflake encourages you to file pull requests early, however please prepend `[WIP]` to work-in-progress pull requests. If your pull request breaks the existing API, prepend `BREAKING` to the title of the pull request. If possible, append a short identifier on what parts of Snowflake was changed, followed by a colon, for example, `Controller:`. If your pull request fixes an issue, indicate which issue was fixed in parentheses after the description of your pull request. 

Your pull request will not be merged until it passes all checks and tests. 

By filing a pull request you agree to both the terms listed in the Mozilla Public License 2.0, and the terms listed below in the Legal section. 

Legal
-----
Snowflake is licensed under the Mozilla Public License 2.0

If you are a new contributor you must append your name and relavent contact information such as an email address in the case of legal notifications to the NOTICE file. You are responsible for keeping this information current. In an event of a legal notice such as a relicensing discussion or similar and a reply has not been received after a period of 60 days after the notice has been sent, you agree to be assumed to vote in favour of whatever legal change is being discussed, and you agree to accept any change in rights you have to your contributions up to and including the forfeiture of all rights to any other party.

