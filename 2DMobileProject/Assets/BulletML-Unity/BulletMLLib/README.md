# BulletMLLib - Unity version

This is a fork of the [dmanning23's BulletMLLib](https://github.com/dmanning23/BulletMLLib).

## Unity version
 
We made several modification to make it compatible with Unity 4.3+:

- Using `UnityEngine` namepsaces
- Using Vector2 for positions
- Added an abstract method to `Bullet.cs` to tell the game when the bullet is ready 

We also fixed:

- The ``times`` node of the ``repeat`` were evaluated only once, so if you were using a `$rank` or `$rand` it always had the same value during runtime.

It is not up to date as the current version is using `async` and some .NET features that are still not available in Unity (.NET 3.5)  .

## Dependencies

This library use **Equationator**, an open-source library to compute mathematical string operations.

We decided to embed the `.dll` file only, but you can find the whole source code on GitHub:

- [Equationator on GitHub](https://github.com/dmanning23/Equationator) 

## Is this a Unity plugin?

**Disclaimer**: this is **NOT** a Unity plugin. This is a C# engine **compatible** with a Unity project.
 
We provide a simple, powerful and fully documented plugin to integrate BulletML in your game:
 
- **[BulletML for Unity](http://pixelnest.io/work/bulletml-for-unity/)**. 


