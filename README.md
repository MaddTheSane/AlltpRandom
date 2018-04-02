# The Legend of Zelda: A Link to the Past Randomizer for Mac

This is a Mac port of [The Legend of Zelda: A Link to the Past Randomizer](https://github.com/Dessyreqt/alttprandomizer) by [Dessyreqt](https://github.com/Dessyreqt) to the Mac.

## The Problem

The problem with the original on the Mac is that it uses WinForms, which does not have a Cocoa Mac backend. This limits it to using the 32-bit runtime, which Apple is starting to deprecate.
That, and there are some graphical problems, as well as not looking very "Mac-like".

## The Solution

The obvious solution would be to port the UI over to Cocoa, and use the existing random generation code. The problem is that it is mostly written in Objective-C. Luckily, Visual Studio for Mac includes *Xamarin.Mac*, which allows C# code to call Cocoa classes and methods.
