using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.WPF.Reflection.Attributes;
using System.Linq;

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
            int count = 0;

            // Helper function to safely find and override existing GMSTs
            void PatchGMST(string editorID, float newValue)
            {
                // Find the existing vanilla GMST in the load order
                var existingGmst = state.LoadOrder.PriorityOrder.GameSetting().WinningOverrides()
                    .FirstOrDefault(x => x.EditorID == editorID);

                if (existingGmst != null)
                {
                    // Override it (this safely copies the vanilla FormKey!)
                    var gmst = state.PatchMod.GameSettings.GetOrAddAsOverride(existingGmst);

                    // Cast to GameSettingFloat to access the Data property
                    if (gmst is GameSettingFloat floatGmst)
                    {
                        floatGmst.Data = newValue;
                        count++;
                    }
                }
            }

            // Apply the patches
            PatchGMST("fCombatDistance", fCombatDistance);
            PatchGMST("fCombatBashReach", fCombatBashReach);

            return count;
        }
    }
}