using BepInEx;
using BepInEx.Configuration;
using NuclearOption.UI;
using UnityEngine;

namespace VirtualJoystickToggle
{
    /// <summary>
    /// Toggles Nuclear Option's "Use Virtual Joystick" setting with any Unity KeyCode
    /// (Mouse0 by default, including every keyboard key), without opening the
    /// settings menu.
    /// </summary>
    [BepInPlugin(Guid, PluginName, PluginVersion)]
    public sealed class VirtualJoystickTogglePlugin : BaseUnityPlugin
    {
        public const string Guid = "com.codebuff.virtualjoysticktoggle";
        public const string PluginName = "VirtualJoystickToggle";
        public const string PluginVersion = "1.0.0";

        // Verified in PlayerSettings.LoadPrefs() and ControlsMenu.SavePlayerSettings():
        // PlayerPrefs.GetInt("VirtualJoystickEnabled") / SetInt(..., 0 or 1).
        private const string VirtualJoystickPrefKey = "VirtualJoystickEnabled";

        private ConfigEntry<KeyboardShortcut> _toggleKey;
        private ConfigEntry<float> _toggleCooldown;

        private float _nextToggleTime;
        private bool _wasFocused = true;

        private void Awake()
        {
            _toggleKey = Config.Bind(
                "Input",
                "ToggleKey",
                new KeyboardShortcut(KeyCode.Mouse0),
                "Key or mouse button that toggles the virtual joystick. " +
                "Any Unity KeyCode works: Mouse0 (left), Mouse1 (right), Mouse2 (middle), " +
                "Mouse3-Mouse6, or any keyboard key such as F9 or V. " +
                "Modifiers go after the main key, for example V + LeftControl. " +
                "Set to None to disable.");

            _toggleCooldown = Config.Bind(
                "Input",
                "ToggleCooldownSeconds",
                0.35f,
                "Minimum number of seconds between toggles. Prevents accidental double toggles.");

            Logger.LogInfo(
                $"{PluginName} {PluginVersion} loaded. " +
                $"Toggle key: {_toggleKey.Value.Serialize()}.");
        }

        private void Update()
        {
            if (!Application.isFocused)
            {
                _wasFocused = false;
                return;
            }

            // Swallow the click/keypress that brings the game window back into focus.
            if (!_wasFocused)
            {
                _wasFocused = true;
                _nextToggleTime = Time.unscaledTime + Mathf.Max(_toggleCooldown.Value, 0.2f);
                return;
            }

            if (!IsTogglePressed(_toggleKey.Value))
            {
                return;
            }

            if (Time.unscaledTime < _nextToggleTime || !CanToggle())
            {
                return;
            }

            Toggle();
        }

        /// <summary>
        /// Edge-triggered check for the configured KeyCode, including mouse buttons.
        /// Deliberately not KeyboardShortcut.IsDown(): that method refuses to fire
        /// while any unrelated key is held, which would block toggling in flight.
        /// Only the configured modifiers must be held.
        /// </summary>
        private static bool IsTogglePressed(KeyboardShortcut shortcut)
        {
            KeyCode mainKey = shortcut.MainKey;
            if (mainKey == KeyCode.None)
            {
                return false;
            }

            bool pressed = IsMouseButton(mainKey)
                ? Input.GetMouseButtonDown(MouseButtonIndex(mainKey))
                : Input.GetKeyDown(mainKey);
            if (!pressed)
            {
                return false;
            }

            foreach (KeyCode modifier in shortcut.Modifiers)
            {
                bool held = IsMouseButton(modifier)
                    ? Input.GetMouseButton(MouseButtonIndex(modifier))
                    : Input.GetKey(modifier);
                if (!held)
                {
                    return false;
                }
            }

            return true;
        }

        // KeyCode.Mouse0 through KeyCode.Mouse6 cover all seven Unity mouse buttons.
        private static bool IsMouseButton(KeyCode key)
        {
            int code = (int)key;
            return code >= (int)KeyCode.Mouse0 && code <= (int)KeyCode.Mouse6;
        }

        private static int MouseButtonIndex(KeyCode key)
        {
            return (int)key - (int)KeyCode.Mouse0;
        }

        /// <summary>
        /// Mirrors the game's own gameplay gate. Every condition below was verified
        /// in the decompiled Assembly-CSharp.dll.
        /// </summary>
        private static bool CanToggle()
        {
            // CursorManager sets Cursor.visible = true for GameMenu, Map, SelectionMenu,
            // Dialogue, Chat, CameraControlUI, Loading, NotInGame and EmptyScene.
            if (Cursor.visible)
            {
                return false;
            }

            // False while paused or during dialogue.
            if (!GameManager.flightControlsEnabled)
            {
                return false;
            }

            // Explicit menu guards used by PilotPlayerState.PlayerAxisControls().
            if (DynamicMap.mapMaximized || RadialMenuMain.IsInUse() || LeaderboardMenu.IsOpen())
            {
                return false;
            }

            // Verified: GameManager.GetLocalAircraft returns the local player's Aircraft.
            if (!GameManager.GetLocalAircraft(out Aircraft aircraft) || aircraft == null)
            {
                return false;
            }

            Pilot[] pilots = aircraft.pilots;
            if (pilots == null || pilots.Length == 0)
            {
                return false;
            }

            // Aircraft.SetupLocalPlayerAndUI() calls pilots[0].SwitchState(pilots[0].playerState)
            // for the local player only, so this is the exact "actively piloting" state.
            Pilot pilot = pilots[0];
            return pilot != null && pilot.currentState is PilotPlayerState;
        }

        private void Toggle()
        {
            bool enable = !PlayerSettings.virtualJoystickEnabled;

            // Verified: this public static field is read every physics tick by
            // PilotPlayerState.PlayerAxisControls() and every frame by
            // CameraCockpitState.UpdateState(), so no restart or Harmony patch is needed.
            PlayerSettings.virtualJoystickEnabled = enable;

            // Verified: ControlsMenu.SavePlayerSettings() persists this as an int 0/1.
            // PlayerSettings.LoadPrefs() is called by every settings menu, so an
            // unpersisted change would be reverted.
            PlayerPrefs.SetInt(VirtualJoystickPrefKey, enable ? 1 : 0);
            PlayerPrefs.Save();

            if (enable)
            {
                // While disabled, PilotPlayerState stops calling SetVirtualJoystick(),
                // so the HUD stick keeps its last deflection. Recenter it to avoid an
                // immediate roll/pitch command when mouse flight is re-enabled.
                FlightHud flightHud = SceneSingleton<FlightHud>.i;
                if (flightHud != null)
                {
                    flightHud.SetVirtualJoystick(Vector3.zero);
                }
            }

            _nextToggleTime = Time.unscaledTime + Mathf.Max(_toggleCooldown.Value, 0f);

            // The game's own HUD message feed, used for "Slow Motion Enabled" etc.
            GameplayUI gameplayUi = SceneSingleton<GameplayUI>.i;
            if (gameplayUi != null)
            {
                gameplayUi.GameMessage(enable ? "Virtual joystick: ON" : "Virtual joystick: OFF");
            }

            Logger.LogInfo($"Virtual joystick {(enable ? "ON" : "OFF")}.");
        }
    }
}
