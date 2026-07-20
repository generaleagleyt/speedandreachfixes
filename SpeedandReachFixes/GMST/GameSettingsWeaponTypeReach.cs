// GameSettings subsection containing reach modifiers.
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace SpeedandReachFixes.GMST
{
    /// <summary>
    /// Contains game settings related to weapon type reach modifiers, specifically:
    ///		fObjectHitWeaponReach,
    ///		fObjectHitTwoHandReach,
    ///	  & fObjectHitH2HReach.
    /// </summary>
    public class GameSettingsWeaponTypeReach
    {
        [MaintainOrder]
        [Tooltip("Enables this category. It is highly recommended that you leave this enabled!")]
        public bool Enabled = true;

        [SettingName("fObjectHitWeaponReach")]
        [Tooltip("Modifier added to the reach of one-handed weapons.")]
        public float fObjectHitWeaponReach = 81F;

        [SettingName("fObjectHitTwoHandReach")]
        [Tooltip("Modifier added to the reach of two-handed weapons.")]
        public float fObjectHitTwoHandReach = 135F;

        [SettingName("fObjectHitH2HReach")]
        [Tooltip("Modifier added to unarmed reach.")]
        public float fObjectHitH2HReach = 61F;

        // Adds the game settings from this class to the current patcher state
        public int AddGameSettings(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            if (!Enabled) return 0;
            var count = 0;

            count += SetFloat(state, "fObjectHitWeaponReach", fObjectHitWeaponReach);
            count += SetFloat(state, "fObjectHitTwoHandReach", fObjectHitTwoHandReach);
            count += SetFloat(state, "fObjectHitH2HReach", fObjectHitH2HReach);

            return count;
        }

        // Overrides an existing float GMST if it exists (reusing its FormKey),
        // otherwise creates a new one. This avoids allocating a null FormKey.
        private static int SetFloat(IPatcherState<ISkyrimMod, ISkyrimModGetter> state, string editorId, float value)
        {
            if (state.LinkCache.TryResolve<IGameSettingGetter>(editorId, out var existing)
                && existing is IGameSettingFloatGetter)
            {
                var gmst = state.PatchMod.GameSettings.GetOrAddAsOverride(existing);
                if (gmst is IGameSettingFloat floatSetting)
                    floatSetting.Data = value;
            }
            else
            {
                var gmst = new GameSettingFloat(state.PatchMod.GetNextFormKey(), state.PatchMod.SkyrimRelease)
                {
                    EditorID = editorId,
                    Data = value
                };
                state.PatchMod.GameSettings.Add(gmst);
            }
            return 1;
        }
    }
}