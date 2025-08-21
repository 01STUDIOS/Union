<p align="center">
  <img align="center" src="img/logo.png">
</p>
<p align="center">
  People Playground Framework created by devs from 01STUDIO
</p>

# Table of Contents
- [About Project](#about-project)
- [Why Use This Library?](#why-use-this-library)
- [Installation](#installation)
- [Features](#features)
- [Documentation](#documentation)

## About Project

The decision to create a common library came when I was working with the Grade system. I realized how terrible and inconvenient creating characters with strong bodies could be. I was particularly frustrated by constantly having to create endless components that would "roll back" the actions of original components like LimbBehaviour and CirculationBehaviour. Everything irritated me, especially the powerlessness to adequately change the logic - like disabling bleeding, or making it so shots could wound a character but collision impacts wouldn't affect them. That's how I started developing Union.

## Why Use This Library?

**1. COMPATIBILITY** - The most important reason for its creation.
Mods that create their own Durability systems, Resist systems, and other systems won't work properly together. If we all use a common solution, it will improve the player experience.

**2. It's FREE** - I've finally freed myself from real problems in my life and looked at the current situation in the modding community. It's as fragmented as possible. I believe we should work together on matters concerning the players' gaming experience.

**3. Completely unobfuscated** - You can analyze the code through any IL analyzer if you need to understand how a particular function works. Even in Visual Studio, you can always hold CTRL and navigate to the decompiled code.

Everyone will find their own reason to use it. At the very least, my reason is the compatibility of my own mods with each other.

## Installation

Most developers have already switched to pre-compiled mods, as they're much more convenient to work with and support much more functionality.

*(Skip this if you know what I'm talking about above)*

If you're still creating three thousand .cs files and writing each one in mod.json, I sincerely feel sorry for you. Look at how our modifications are implemented - we use Starter.cs which injects the mod library and uses reflection to launch the initialization method. You can freely open the source code of our public modifications. For .dll analysis, use dnSpy (https://github.com/dnSpy/dnSpy) or any other IL analyzer.

### Step 1: Add Libraries to Project References

There are 3 libraries in total:
- **ZeroOne.Union** - The main library inseparably linked with People Playground
- **ZeroOne.UI** - Used by Union in some places (e.g., Debug Human)
- **ZeroOne.Union.Bridge** - The link between Unity Editor (yes, Union has some things that are added to the Unity editor, mainly for quick clothing creation in Cloth Module)

*Note: Union won't work without Union.Bridge (mostly)*

### Step 2: Connect the Library to the Game

Before injecting your mod, you must inject Union and Union.Bridge first. We have **ZeroOne.Updater** for this - the simplest dependency manager for People Playground.

Let's imagine we have an empty mod:

1. Create `Starter.cs`
2. I've put all necessary files in `samples/mod`
3. If you took my Starter.cs, then change:
```csharp
private const string MOD_ENTRY = "YourModNamespace.YourEntryPoint";
private const string MOD_DLL_NAME = "YourMod.dll";
```
to your own data.

The dependency manager looks for the `dependencyConfig.json` file in the mod folder, downloads it, checks the relevance of libraries, and initializes them. The Runtime folder contains Union's own dependencies, which are automatically loaded into the game in my Starter.cs.

That's it! Your mod waits for Union to load before loading itself.

## Features

While there aren't many features yet, here are some highlights:

- **Durability System** - Universal and simple system for editing creature characteristics
- **Resist System** - We all know Mahoraga, this is exactly for those cases
- **Cloth Module** - Super simple and convenient clothing system, both regular sprites and with physics
- **Custom System** - I've seen how many people load Asset Bundles and it's just terrible. This is a simple manager for those who don't want to bother with resource organization. Just drop the Asset Bundle in the folder, Union will load it itself
- **SmartData** - "Smart" data types. For example, when you make multipliers in components and need to influence them from another place
- **Skin System** - Simple skin system. Just add skins to the folder like in JJP, Union will load and apply them for characters with the component
- Plus a bunch of other utilities that I don't see the point in writing about separately

*Important: This is all very brief. To understand how the library works, you need to read the documentation.*

## [Documentation](https://01studio.dev/union)