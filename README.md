# FrizzQOL Fermenter Control

https://github.com/frizzlebeard/FrizzQOL.FermenterControl

> Set how long the fermenter takes, and see the time left in color when you look at it. 🍯

---

## What it does

Each fermenter uses the time you set. While a batch is working, looking at it adds the time left on its own line, in the color you pick.

Out of the box the time stays the normal Valheim time. The extra line is orange until you change it.

## Config

`BepInEx/config/com.frizzqol.fermentercontrol.cfg`

| Setting | Default | What it does |
| --- | --- | --- |
| Minutes | 0 | Real minutes until a batch is ready. 0 keeps the normal time. Below 0 does the same. |
| TimeColor | orange | Color of that time. A Unity color name, or `#RRGGBB`. |

A bad color falls back to orange.

## Multiplayer

Install this on the dedicated server and on every client. Use the same `Minutes` on each of them. `TimeColor` is only for you.

## Install

Install with r2modman or the Thunderstore Mod Manager.

To install by hand, copy `FrizzQOL.FermenterControl.dll` into `BepInEx/plugins`.

## Requirements

- Valheim
- [BepInExPack for Valheim](https://thunderstore.io/c/valheim/p/denikson/BepInExPack_Valheim/)

## Support

☕ If you enjoy my work, please buy me a coffee.

Cash App: `$FrizzleFry4`

## License

[MIT](LICENSE). You can use, copy, change, and share this mod. Keep the copyright notice with any copy.

## Building

1. Copy `Environment.props.example` to `Environment.props`.
2. Set your Valheim and BepInEx folders in that file.
3. From this folder, run:

```
dotnet build FermenterControl.sln -c Release
```

The plugin file is `FrizzQOL.FermenterControl.dll`, under the project `bin\Release\net48` folder.

`Environment.props` stays on your machine. It is listed in `.gitignore`.
