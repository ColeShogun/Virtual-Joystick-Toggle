# AI Assistance Disclaimer

Parts of this project were developed with the assistance of AI coding tools, including
Codebuff. AI assistance was used for decompiled-API analysis, code generation, refactoring,
and documentation.

The maintainer reviewed the code, compiled the plugin, and tested its functionality in
Nuclear Option **0.34.6** before the public 1.0.0 release. The public build differs from the
tested build only in version metadata and documentation; the plugin logic is unchanged.

AI-generated code can still contain mistakes, unsafe assumptions, or outdated API usage.
Game updates can also change the members this mod relies on. As a result:

- The software is provided **as is**, without warranty of any kind.
- You use the mod at your own risk.
- You should review the source before running it if you have specific security or
  compatibility requirements.
- Bug reports with a BepInEx log excerpt and game version are welcome via the
  [issue tracker](https://github.com/ColeShogun/Virtual-Joystick-Toggle/issues).

The mod does not modify game assemblies. It reads and writes the game's existing
`PlayerSettings.virtualJoystickEnabled` value and its `PlayerPrefs` entry at runtime.

For the license terms, see [LICENSE](LICENSE).
