#nw-edge-example

An example usage of Node-Webkit and EdgeJS. This code calls a function inside a .NET class library. I've included everything (except Node-Webkit) so this example code should work out of the box.

<img src="https://github.com/frankhale/nw-edge-example/blob/master/edge-test.png?raw=true" alt="screenshot"/>

###Usage

You need Node-Webkit and Windows to run this.

- Clone or download a zip of this repository
- Unzip into it's own folder (assuming folder named edge-test-app)
- Unzip node-webkit
- Copy edge-test-app folder to node-webkit folder
- Open command prompt to node-webkit
- Run using the command: nw edge-test-app

###C# Class Library

The SimpleLibrary.dll is built from the Hello.cs source file. I've omitted the Visual Studio project to build this from this repository. This library only contains one function as noted in the Hello.cs. Notice the function signature is special because by default Edge.JS wants to call asynchronous methods so it does not block the Javascript engine. 

If you want to see more examples of using Edge.JS please see it's Github repository located at:

https://github.com/tjanczuk/edge

###Building Your Own Edge.JS

Edge.JS has to be rebuilt using nw-gyp in order for it to work from within Node-Webkit. By default Edge.JS is built using node-gyp and only works within Node.JS.

Things you need in order to build Edge.JS yourself:

- Windows (obviously, LOL!)
- Windows 7.1 SDK
- Visual C++ 2010 Express (or any of the paid for versions)
- Python 2.7.x
- Node.JS (I use the x86 version since Node-Webkit is 32bit on Windows).
- nw-gyp (node module needed for configuring the build and building)

Next, open a command prompt to your source code directory and install the Edge.JS module using npm:

```
c:\>cd my-app
c:\my-app>npm install edge
```

This will create a node-modules folder and then install edge into that folder. Now we need to recompile Edge using nw-gyp (instead of node-gyp which it is built with by default).

(assuming Node-Webkit 0.8.4 is the target version of Node-Webkit)

```
c:\my-app\>cd node_modules\edge
c:\my-app\node_modules\edge>nw-gyp configure --target=v0.8.4
c:\my-app\node_modules\edge>nw-gyp build
```

Once the build finishes, the new module will be in:

```
c:\my-app\node_modules\edge\build\Release
```

Copy edge.node to:

```
c:\my-app\node_modules\edge\lib\native\win32\ia32\0.10.0
```

###Author

Frank Hale &lt;frankhale@gmail.com&gt;  
16 January 2014

###License 

GPL version 3, see LICENSE file for details