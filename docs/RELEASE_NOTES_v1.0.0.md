# VirtualJoystickToggle v1.0.0

Toggle Nuclear Option's built-in **Use Virtual Joystick** setting with **Mouse 0**
(left click) — or any key you choose — without opening the Settings menu.

> **This is not a new joystick system.** The mod changes the game's existing
> `PlayerSettings.virtualJoystickEnabled` setting and writes the same `PlayerPrefs` value
> as the Controls menu. Nuclear Option's own mouse-flight code does the rest.

## Asset

- **`VirtualJoystickToggle.dll`**
- SHA-256: `c7dcc472a0265b893cd991c1e7dc69184f262d239b599c4a1c4d6226f912412a`

Only the plugin DLL is included. Game assemblies and BepInEx binaries are **not**
redistributed.

## Installation

1. Install **BepInEx 5.4.x x64** into your Nuclear Option folder if you have not already.
   Download it from [BepInEx releases](https://github.com/BepInEx/BepInEx/releases).
   After extraction, `BepInEx\`, `doorstop_config.ini`, and `winhttp.dll` should sit next to
   `NuclearOption.exe`.
2. Launch the game once, then close it so BepInEx creates its folders.
3. Create this folder:

   ```
   Nuclear Option\BepInEx\plugins\VirtualJoystickToggle\
   ```

4. Copy `VirtualJoystickToggle.dll` into that folder.
5. Launch the game and check `BepInEx\LogOutput.log` for:

   ```
   VirtualJoystickToggle 1.0.0 loaded. Toggle key: Mouse0.
   ```

The default toggle is **Mouse 0**. It works only while actively piloting an aircraft with
the cursor hidden; menus, the map, chat, pause, the editor, and spectator states are
ignored.

## Configuration

The plugin generates:

```
Nuclear Option\BepInEx\config\com.codebuff.virtualjoysticktoggle.cfg
```

Default:

```ini
[Input]
ToggleKey = Mouse0
ToggleCooldownSeconds = 0.35
```

`ToggleKey` accepts any Unity `KeyCode`, for example `Mouse1`, `F9`, `V`, `Keypad7`, or
`V + LeftControl` (Ctrl+V). The main key comes first; modifiers follow. Set
`ToggleKey = None` to disable. Use exact, case-sensitive Unity key names, and restart the
game after editing.

## Compatibility

| Component | Requirement |
|---|---|
| Nuclear Option | **0.34.6** (built and verified against this version) |
| BepInEx | **5.4.x x64** (tested with 5.4.23.4) |
| Runtime | Unity 2022.3.62, Mono x64 |
| OS | Windows |

BepInEx 6 and IL2CPP are not supported. Nuclear Option is in early access; a future update
may require a plugin rebuild.

## Features

- Toggle the game's built-in mouse-flight setting in flight.
- Configurable binding for keyboard keys, mouse buttons, and joystick buttons.
- Immediate effect; no mission restart or settings menu.
- Persists through `PlayerPrefs` and stays in sync with the Controls checkbox.
- Recenters the HUD stick when re-enabling mouse flight.
- HUD notification: `Virtual joystick: ON` / `OFF`.
- Guards against toggling in menus, the map, chat, pause, the editor, or while not piloting.
- Edge-triggered input, a configurable cooldown, and a focus-regain guard.

## Known limitations

- Verified against Nuclear Option 0.34.6 only.
- BepInEx 5.4.x x64 only; BepInEx 6 is not supported.
- The HUD notification uses the game's message system and lasts roughly 8 seconds plus
  0.1 seconds per character.
- If a game action is bound to the configured toggle key, both actions fire.
- There is no in-game rebinding UI in this mod; edit the cfg file.
- Extra mouse buttons (`Mouse3`–`Mouse6`) depend on mouse and driver support.
- The setting is local and client-side; it is not synchronized between players.
- Uninstalling leaves the last virtual-joystick state in `PlayerPrefs`; change it in the
  game's Controls menu if desired.
- This is not a new joystick, flight model, or input remapper.

## Changelog

### 1.0.0 — 2026-11-09

Initial public release.

- Toggle the built-in **Use Virtual Joystick** setting with Mouse 0 by default.
- Bind to any Unity `KeyCode`, including keyboard keys, mouse buttons, and joystick buttons.
- Optional modifier combinations such as `V + LeftControl`.
- Immediate application and `PlayerPrefs` persistence.
- HUD stick recentering and ON/OFF notification.
- Gameplay-state guards, edge-triggered input, cooldown, and focus-regain guard.
- Documentation, configuration example, build instructions, and troubleshooting guide.

## AI assistance and license

Parts of this project were developed with AI coding assistance. The maintainer reviewed,
compiled, and tested the functionality in Nuclear Option 0.34.6. See
[AI_DISCLAIMER.md](https://github.com/ColeShogun/Virtual-Joystick-Toggle/blob/main/AI_DISCLAIMER.md)
for the full disclosure.

Released under the [MIT License](https://github.com/ColeShogun/Virtual-Joystick-Toggle/blob/main/LICENSE).
Nuclear Option is © Shockfront Studios Pty Ltd. This project is not affiliated with or
endorsed by Shockfront Studios and does not redistribute game or BepInEx binaries.
