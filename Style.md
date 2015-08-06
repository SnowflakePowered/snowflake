Coding Style
============

C#
--
Snowflake uses C# 6.0 You will need Visual Studio 2015 Community Edition to build Snowflake.
Use Visual Studio 2015 defaults - Follow [MSDN C# Coding Conventions](http://msdn.microsoft.com/en-us/library/ff926074.aspx) where possible except if specified below.

- Use var when creating a new object, specify type otherwise

        var x = new List<string, string>();
        var x = new Foo();

        Bar x = Foo.GetBar();
        Bar x = Foo.Bar;
        Bar x = Foo.SomeObject;


- Use explicit instantiation when creating flat arrays

        var x = new string[] {'foo', 'bar'};

- Prepend all object-level properties, fields and methods with `this.`
- Prepand all static properties properties, fields and methods with the class name
- Never use static import, always import the full class
- Never use string concatenation or String.Format if a string literal is involved. Only string interpolation is allowed.
- Use null propagation when possible. There should not be any other forms of null checking
- If your class implements an interface, do not document implementation methods. Instead, document only the interface.
- Always use Allman bracket style. For example
```c#
if (condition)
{
    //good
}

if(condition) {
  //bad
}
```

HTML and Javascript
-------------------
Follow [Google HTML/CSS Style Guide](https://google-styleguide.googlecode.com/svn/trunk/htmlcssguide.xml) where possible except if specified below.

Use K&R bracket placement.
```js
if (condition) {
  //good
}
if (condition)
{
  //bad
}
```

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
