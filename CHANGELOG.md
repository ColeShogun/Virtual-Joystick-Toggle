# Changelog

All notable changes to this project are documented here.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-11-09

### Added

- Initial public release.
- Toggle Nuclear Option's built-in **Use Virtual Joystick** setting in flight with
  **Mouse 0** (left click) by default.
- Configurable toggle binding using any Unity `KeyCode`: keyboard keys, function keys,
  numpad, modifiers, `Mouse0`–`Mouse6`, and joystick buttons.
- Optional modifier combinations such as `V + LeftControl`.
- Immediate application on the next physics tick, with no mission restart or settings menu.
- Persistence through `PlayerPrefs` (`VirtualJoystickEnabled`), keeping the Controls menu
  checkbox in sync.
- HUD stick recentering when mouse flight is re-enabled.
- In-game HUD notification (`Virtual joystick: ON` / `OFF`).
- Guards against toggling in menus, the map, chat, pause, the mission editor, spectator
  states, and while not actively piloting.
- Edge-triggered input, a configurable cooldown, and a focus-regain guard to prevent
  accidental toggles.
- Reference configuration example, build instructions, and troubleshooting documentation.

### Compatibility

- Nuclear Option **0.34.6**
- BepInEx **5.4.x x64** (tested with 5.4.23.4)
- Unity 2022.3.62, Mono x64

[1.0.0]: https://github.com/ColeShogun/Virtual-Joystick-Toggle/releases/tag/v1.0.0
