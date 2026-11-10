# VirtualJoystickToggle

A BepInEx 5 quality-of-life mod for **Nuclear Option** that lets you toggle the game's
built-in **Use Virtual Joystick** setting with **Mouse 0** (left click) — or any key you
choose — without opening the Settings menu.

[Download the latest release](https://github.com/ColeShogun/Virtual-Joystick-Toggle/releases) ·
[Report an issue](https://github.com/ColeShogun/Virtual-Joystick-Toggle/issues) ·
[Changelog](CHANGELOG.md)

> **This mod does not add a new joystick or flight model.** It flips the game's existing
> `PlayerSettings.virtualJoystickEnabled` flag and writes the same `PlayerPrefs` value the
> Controls menu writes. Nuclear Option's own mouse-flight code does the rest. If you turn
> the setting off in the game's Controls menu, the mod is off too — there is only one
> setting, and the mod simply changes it while you fly.

---

## Features

- Toggle the game's built-in virtual joystick with **Mouse 0** by default.
- Bind the toggle to **any Unity `KeyCode`**: keyboard keys, function keys, numpad,
  modifiers, mouse buttons, or joystick buttons.
- Applies immediately on the next physics tick — no mission restart, no settings menu.
- Persists the change through `PlayerPrefs`, keeping the Controls menu checkbox in sync.
- Recenters the HUD stick when mouse flight is re-enabled, preventing a sudden
  roll/pitch command from the stick's previous deflection.
- Shows the game's own HUD notification: `Virtual joystick: ON` / `Virtual joystick: OFF`.
- Refuses to toggle in menus, the map, chat, pause, the mission editor, or while not
  actively piloting an aircraft.
- Edge-triggered input, a configurable cooldown, and a focus-regain guard prevent
  accidental repeated toggles.
- No Harmony patches, detours, or transpilers.

---

## Compatibility

| Component | Requirement |
|---|---|
| **Nuclear Option** | **0.34.6** — the version this release was built and verified against |
| **BepInEx** | **5.4.x, 64-bit (Mono)** — tested with **5.4.23.4** |
| **Game runtime** | Unity 2022.3.62, Mono x64 |
| **OS** | Windows (the game's supported PC platform) |
| **Build target** | .NET Standard 2.1 |

BepInEx **6** and IL2CPP builds are **not** supported. Nuclear Option is a Mono game, and
this plugin targets the BepInEx 5 API.

Nuclear Option is in early access and updates frequently. A future game update may rename
or remove the members this mod uses. If the plugin stops working after an update, check the
[BepInEx log](#troubleshooting) and open an issue with the game version and log excerpt.

---

## Installation

### 1. Install BepInEx 5 (skip if already installed)

1. Download the **BepInEx 5.4.x x64** release from
   [github.com/BepInEx/BepInEx/releases](https://github.com/BepInEx/BepInEx/releases).
   The file is usually named `BepInEx_win_x64_5.4.23.4.zip` (version may differ).
2. In Steam, right-click **Nuclear Option** → **Manage** → **Browse local files**.
   This opens the game folder containing `NuclearOption.exe`.
3. Extract the **contents** of the BepInEx ZIP directly into that folder. Afterward you
   should see `BepInEx\`, `doorstop_config.ini`, and `winhttp.dll` next to
   `NuclearOption.exe`.
4. Launch the game once, then close it. BepInEx creates its folders and
   `BepInEx\LogOutput.log`.

> If you already have BepInEx 5 working for other Nuclear Option mods, skip this section.

### 2. Install VirtualJoystickToggle

1. Download **`VirtualJoystickToggle.dll`** from the
   [latest release](https://github.com/ColeShogun/Virtual-Joystick-Toggle/releases).
2. Create this folder if it does not exist:

   ```
   Nuclear Option\BepInEx\plugins\VirtualJoystickToggle\
   ```

3. Copy `VirtualJoystickToggle.dll` into it:

   ```
   Nuclear Option\BepInEx\plugins\VirtualJoystickToggle\VirtualJoystickToggle.dll
   ```

4. Launch the game.

BepInEx scans plugin subfolders, so the folder name is not critical, but using a dedicated
folder makes uninstalling clean.

### 3. Verify it loaded

Open `Nuclear Option\BepInEx\LogOutput.log` and search for:

```
VirtualJoystickToggle 1.0.0 loaded. Toggle key: Mouse0.
```

The exact logger prefix varies by BepInEx version, but the message text should match. If
the line is missing, see [Troubleshooting](#troubleshooting).

---

## Usage

- **Default toggle:** **Mouse 0** (left click).
- **When it works:** only while you are actively piloting an aircraft, with the cursor
  hidden and no blocking menu open.
- **Feedback:** the game's HUD message feed shows `Virtual joystick: ON` or
  `Virtual joystick: OFF`.
- **Persistence:** the setting is saved to `PlayerPrefs`, so it survives restarts and
  matches the checkbox in **Settings → Controls**.
- **Cooldown:** 0.35 seconds by default, to absorb double-clicks and bounced inputs.

The left mouse button is unbound in Nuclear Option's default flight controls (`Fire` is
Space and `Free Look` is the right mouse button), which is why it is a safe default. If
you have rebound a game action to Mouse 0, change the toggle key — see below.

---

## Configuration

The plugin creates its config on first launch:

```
Nuclear Option\BepInEx\config\com.codebuff.virtualjoysticktoggle.cfg
```

Close the game before editing it. The default contents are:

```ini
[Input]

## Key or mouse button that toggles the virtual joystick. Any Unity KeyCode works:
## Mouse0 (left), Mouse1 (right), Mouse2 (middle), Mouse3-Mouse6, or any keyboard
## key such as F9 or V. Modifiers go after the main key, for example V + LeftControl.
## Set to None to disable.
# Setting type: KeyboardShortcut
# Default value: Mouse0
ToggleKey = Mouse0

## Minimum number of seconds between toggles. Prevents accidental double toggles.
# Setting type: Single
# Default value: 0.35
ToggleCooldownSeconds = 0.35
```

A copy is included as [`config.example.cfg`](config.example.cfg).

### Change the toggle key

Set `ToggleKey` to a Unity `KeyCode`. Changes take effect on the next game launch.

| Desired toggle | Value |
|---|---|
| Left click (default) | `Mouse0` |
| Right click | `Mouse1` |
| Middle click | `Mouse2` |
| Extra mouse buttons | `Mouse3` … `Mouse6` |
| F9 | `F9` |
| The V key | `V` |
| Numpad 7 | `Keypad7` |
| Ctrl+V | `V + LeftControl` |
| Joystick button 0 | `JoystickButton0` |
| Disable the toggle | `None` |

Use Unity's exact, case-sensitive spelling. `f9` is not the same as `F9`.

**Shortcut syntax:** the **main key comes first**, followed by modifiers. For `V +
LeftControl`, hold Ctrl and then press V. The main key is the one that must be pressed
last. The plugin requires only the configured modifiers to be held; unrelated keys you are
already holding (W, Space, etc.) do not block the toggle.

### Cooldown

`ToggleCooldownSeconds` is the minimum time between successful toggles. The default
`0.35` is enough to swallow a double-click. Set it to `0` only if you want no debounce.

> If you use **BepInEx Configuration Manager**, the entries may also appear in its in-game
> menu. Editing the cfg directly is always supported.

---

## How it works

The mod uses the game's own setting and input path; it does not create a parallel joystick
system.

1. On a valid press it flips the public static field
   `PlayerSettings.virtualJoystickEnabled`.
2. It writes `PlayerPrefs.SetInt("VirtualJoystickEnabled", on ? 1 : 0)` and calls
   `PlayerPrefs.Save()`, exactly mirroring `ControlsMenu.SavePlayerSettings()`.
3. It deliberately does **not** call `PlayerSettings.LoadPrefs()`. That method also reloads
   graphics and detail settings (`GraphicsHelper.Load()` → `ApplyAll()` and
   `DetailSettings.Load()`), which is not safe mid-flight. Writing the field and the pref
   directly gives the same result without touching video settings.
4. When enabling, it calls `FlightHud.SetVirtualJoystick(Vector3.zero)` so the HUD stick
   does not resume from its last deflection.
5. It shows the result through `GameplayUI.GameMessage(...)`, the same HUD message feed used
   for messages such as “Slow Motion Enabled”.

The setting is read by `PilotPlayerState.PlayerAxisControls()` on every physics tick and by
`CameraCockpitState.UpdateState()` every frame, which is why the change applies immediately.

All game members used by the plugin were checked against the decompiled
`Assembly-CSharp.dll` for Nuclear Option **0.34.6**. No game assembly is modified, and no
game code or assets are redistributed.

---

## Toggle conditions

The plugin toggles only when **all** of these are true:

- The game window is focused, and the click/keypress that restored focus has been swallowed.
- `Cursor.visible == false` — not in a menu, map, chat, pause, loading screen, or editor.
- `GameManager.flightControlsEnabled` — the game is not paused and not in dialogue.
- `DynamicMap.mapMaximized == false`.
- `RadialMenuMain.IsInUse() == false`.
- `LeaderboardMenu.IsOpen() == false`.
- `GameManager.GetLocalAircraft(out aircraft)` succeeds.
- `aircraft.pilots[0].currentState is PilotPlayerState` — you are actively piloting, not
  spectating or in a deployed/limbo state.
- At least `ToggleCooldownSeconds` has passed since the last successful toggle.

These guards intentionally make the mod conservative: a click in a menu, map, or chat box
will never toggle mouse flight.

---

## Troubleshooting

### The mod does not appear in the log

- Confirm BepInEx itself is working. `BepInEx\LogOutput.log` should exist and mention
  BepInEx 5.4.x.
- Confirm the DLL is at
  `BepInEx\plugins\VirtualJoystickToggle\VirtualJoystickToggle.dll`, not loose in the game
  root or inside `BepInEx\core`.
- Confirm you installed the **x64** BepInEx 5 build. A 32-bit BepInEx will not load into the
  64-bit game.
- If you use a mod manager or a custom loader, verify it scans this plugin folder.
- Remove any duplicate copies of `VirtualJoystickToggle.dll`.
- If Windows marked the downloaded DLL as blocked, open its **Properties** and choose
  **Unblock**, then restart the game.

### The plugin loads, but Mouse 0 does nothing

- The toggle only works while **actively piloting an aircraft** with the cursor hidden.
  It will not fire in the hangar, menu, map, spectator view, or mission editor.
- Make sure the map is closed, no radial menu is open, and the leaderboard is closed.
- If you rebound a game action to Mouse 0, both actions will fire. Change `ToggleKey` to
  another mouse button or a keyboard key.
- Wait at least `ToggleCooldownSeconds` (0.35 s by default) between attempts.
- Check that the log line prints the key you expect:
  `Toggle key: Mouse0.` If it prints a different key, the cfg value is different from what
  you intended.

### The config value is ignored or BepInEx logs a keybind error

- Use exact Unity `KeyCode` names and capitalization: `Mouse0`, `F9`, `LeftControl`,
  `Keypad7`, not `mouse0`, `f9`, or `LCtrl`.
- Put the main key first: `V + LeftControl`, not `LeftControl + V`.
- Set `ToggleKey = None` to disable the toggle entirely.
- After editing, save the file and restart the game. The config is read at startup.

### The setting changes by itself

The plugin writes the same `PlayerPrefs` value as the Controls menu. If you change the
checkbox in the game's settings, the mod observes that value; they are the same setting.
If the value seems to revert after a crash, check that the game exited normally so
`PlayerPrefs.Save()` could run.

### It worked before a game update

Nuclear Option is updated frequently. A game update can rename or remove the members this
mod relies on. Check `BepInEx\LogOutput.log` for exceptions, then open an issue with:

- Nuclear Option version
- BepInEx version
- The relevant log excerpt
- Whether the plugin loaded at all

### Multiplayer

The mod changes a client-side input setting only. It does not send network messages and
does not alter aircraft performance or game rules. Use mods only in environments where
they are permitted.

---

## Known limitations

- Verified against Nuclear Option **0.34.6** only. Newer builds may work but are not
  guaranteed until tested.
- Requires **BepInEx 5.4.x x64**; BepInEx 6 is not supported.
- The ON/OFF notification uses the game's message system, whose lines last roughly
  8 seconds plus 0.1 seconds per character. It is visible but not instant-dismiss.
- If a game action is bound to the configured toggle key, both the action and the toggle
  will fire. Choose a different key.
- There is no in-game rebinding UI in this mod; edit the cfg file. BepInEx Configuration
  Manager may expose the entries if you use it.
- Extra mouse buttons (`Mouse3`–`Mouse6`) depend on the mouse and driver reporting them to
  Unity.
- The setting is local and client-side; it is not synchronized between players.
- Uninstalling the plugin leaves the last virtual-joystick state in `PlayerPrefs`. Change it
  in the game's Controls menu if you want a different default.
- This is not a new joystick, flight model, or input remapper. It only toggles the game's
  existing mouse-flight setting.

---

## Uninstall

1. Close Nuclear Option.
2. Delete the folder:

   ```
   Nuclear Option\BepInEx\plugins\VirtualJoystickToggle\
   ```

3. Launch the game. The virtual joystick remains in whatever state it was last set to.
   Change it in **Settings → Controls** if desired.
4. BepInEx itself can stay installed for other mods; removing it is outside this mod's
   scope.

Deleting the plugin does not delete the game's `PlayerPrefs` entry, by design — it is the
game's own setting, not a mod-specific value.

---

## Building from source

Requirements:

- Windows with Nuclear Option **0.34.6** installed
- BepInEx **5.4.x x64** installed in the game folder
- .NET SDK capable of targeting **.NET Standard 2.1** (built with 9.0.123)
- Visual Studio 2022 is optional; the solution is included

The build references the game's managed assemblies and `BepInEx.dll` from your local
installation. Those proprietary binaries are **not** included in this repository and are
never copied into the build output.

```powershell
# From the repository root
dotnet build VirtualJoystickToggle.csproj -c Release `
  -p:GameDir="C:\Program Files (x86)\Steam\steamapps\common\Nuclear Option"
```

Or set the environment variable once:

```powershell
$env:NUCLEAR_OPTION_DIR = "C:\Program Files (x86)\Steam\steamapps\common\Nuclear Option"
dotnet build VirtualJoystickToggle.csproj -c Release
```

Build and install directly into the game:

```powershell
dotnet build VirtualJoystickToggle.csproj -c Release `
  -p:GameDir="C:\Program Files (x86)\Steam\steamapps\common\Nuclear Option" `
  -p:DeployToGame=true
```

Output: `bin\Release\VirtualJoystickToggle.dll`.

The build emits one `MSB3277` warning about `System.IO.Compression` versions. It originates
from `Mirage.dll` being compiled against a newer framework assembly; the plugin does not use
compression and the warning is harmless.

---

## Repository layout

| Path | Purpose |
|---|---|
| `Plugin.cs` | Plugin source |
| `VirtualJoystickToggle.csproj` | SDK-style project; references the local game and BepInEx |
| `VirtualJoystickToggle.sln` | Visual Studio solution |
| `config.example.cfg` | Reference copy of the generated configuration |
| `CHANGELOG.md` | Version history |
| `AI_DISCLAIMER.md` | AI-assistance disclosure |
| `LICENSE` | MIT license |
| `docs/RELEASE_NOTES_v1.0.0.md` | GitHub release description |

---

## AI assistance

Parts of this project were developed with AI coding assistance. See
[AI_DISCLAIMER.md](AI_DISCLAIMER.md) for the full disclosure. In short: AI tools helped
with decompiled-API analysis, code generation, and documentation; the maintainer reviewed,
compiled, and tested the mod. AI can still make mistakes, so use the mod at your own risk
and report problems through the issue tracker.

---

## License

This project is released under the [MIT License](LICENSE).

Nuclear Option is © Shockfront Studios Pty Ltd. BepInEx is © its respective contributors.
This project is not affiliated with or endorsed by Shockfront Studios, and it does not
redistribute game or BepInEx binaries.
