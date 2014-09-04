Coding Style
============

No empty newlines in all parts of the code

C#
--
Use Visual Studio defaults - Follow [MSDN C# Coding Conventions](http://msdn.microsoft.com/en-us/library/ff926074.aspx) where possible except if specified below.

- Use var when creating a new object, specify type otherwise

        var x = new List<string, string>();
        var x = new Foo();
    
        Bar x = Foo.GetBar();
        Bar x = Foo.Bar;
        Bar x = Foo.SomeObject;


- Use explicit instantiation when creating flat arrays

        var x = new string[] {'foo', 'bar'};

- Prefer using keywords when performing LINQ. Always wrap in parentheses

        var style = (from foo in bar where foo.baz = "foobaz" select foo.qux)

HTML and Javascript
-------------------
Follow [Google HTML/CSS Style Guide](https://google-styleguide.googlecode.com/svn/trunk/htmlcssguide.xml) where possible except if specified below.

Always tabs over spaces

Any theme objects that do not have a defined API must go under `snowflake.theme.heap`
Avoid node.js when theming. Only `snowflake.js` may use node.js.
Do not expose anything to the global scope. Instead expose under `snowflake.theme.heap`

HTML `id` properties must be camelCase. 
Class names must be separated with a hyphen.

Polymer exposed attributes and bound properties must be camelCase. 
`Polymer();` must be invoked inline.
CSS for a Web Component must be contained inline in a single `<style>` element.
Polymer elements must be done declaratively and available as an HTML import.

Do not use vendor prefixes at all, besides `-webkit` if nescessary. 
