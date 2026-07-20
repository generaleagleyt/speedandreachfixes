// GameSettings subsection containing reach modifiers.
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.WPF.Reflection.Attributes;
using System.Linq;

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
            PatchGMST("fObjectHitWeaponReach", fObjectHitWeaponReach);
            PatchGMST("fObjectHitTwoHandReach", fObjectHitTwoHandReach);
            PatchGMST("fObjectHitH2HReach", fObjectHitH2HReach);

            return count;
        }
    }
}