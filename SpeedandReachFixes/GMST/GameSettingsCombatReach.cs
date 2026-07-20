using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace SpeedandReachFixes.GMST
{
    /// <summary>
    /// Contains game settings related to base reach multipliers, specifically fCombatDistance and fCombatBashReach.
    /// </summary>
    public class GameSettingsCombatReach
    {
        [MaintainOrder]

        [Tooltip("Enables this category. It is highly recommended that you leave this enabled!")]
        public bool Enabled = true;

        [SettingName("fCombatDistance")]
        [Tooltip("The base reach multiplier used for all attacks, except for shield bashes.")]
        public float fCombatDistance = 141F;

        [SettingName("fCombatBashReach")]
        [Tooltip("The base reach multiplier used for shield bash attacks.")]
        public float fCombatBashReach = 61F;

        public int AddGameSettings(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            if (!Enabled) return 0;
            var count = 0;

            count += SetFloat(state, "fCombatDistance", fCombatDistance);
            count += SetFloat(state, "fCombatBashReach", fCombatBashReach);

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