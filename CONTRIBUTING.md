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

Before accepting your contribution, you must agree to the following terms below. 

1. You **MUST** append your full legal name, an email through which you may be contacted by the Maintainers, and your GitHub username to the **NOTICE** file. You are responsible for keeping this information current and up-to-date.
2. You assert that you own the copyright covering any and all contributions made under your name to Snowflake.
3. You retain ownership of the copyright covering any and all contributions made under your name to Snowflake, subject to the terms of this agreement.
4. In circumstances under which your rights covering any and all contributions made under your name to Snowflake may change, including but not limited to sublicensing the Project, adding a license to the entirety of the Project, relicensing the entirety of the Project, changing the terms of this agreement, or other such situation where your rights, including any copyrights and patent rights you may own in Snowflake may be changed, you will be contacted through the information provided in the **NOTICE** file by the Maintainers. You will have a maximum of 60 days to raise any objections with the change. You retain the right to object to such change within 60 days of the announcement of any terms of the changes that are proposed, and in such case any and all contributions to Snowflake made under your name will be removed from the project should the changes be made after a period of 60 days.
5. In the event you are uncontactable through the information provided in the **NOTICE** file, or if for any other reason no reply has been received by the Maintainers from you regarding an objection to any change in rights you have to your contributions after a period of 60 days from which the terms of the changes were announced, you agree to accept any and all changes to the rights you have to your contributions, up to and including granting the Maintainers and recipients of the software a perpetual, worldwide, non-exclusive, no-charge, royalty-free, irrevocable copyright license to reproduce, prepare derivative works of, publicly display, publicly perform, sublicense, relicense, and distribute any and all contributions made under your name to Snowflake and such derivative works.

