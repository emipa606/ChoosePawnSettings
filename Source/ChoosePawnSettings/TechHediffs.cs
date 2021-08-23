using System.Collections.Generic;
using ChoosePawnSettings.Settings;

namespace ChoosePawnSettings
{
    public static class TechHediffs
    {
        public static readonly Dictionary<string, float> VanillaTechHediffsChances =
            new Dictionary<string, float>();

        static TechHediffs()
        {
        }

        public static void Initialize()
        {
            saveVanillaTechHediffsValues();
            setCustomTechHediffsValues();
        }

        private static void saveVanillaTechHediffsValues()
        {
            foreach (var pawnKindDef in Main.AllPawnKinds)
            {
                VanillaTechHediffsChances[pawnKindDef.defName] = pawnKindDef.techHediffsChance;
            }
        }

        private static void setCustomTechHediffsValues()
        {
            if (ChoosePawnSettings_Mod.instance?.Settings?.CustomTechHediffsChances == null)
            {
                return;
            }

            var counter = 0;
            foreach (var pawnKindDef in Main.AllPawnKinds)
            {
                if (!ChoosePawnSettings_Mod.instance.Settings.CustomTechHediffsChances.ContainsKey(pawnKindDef
                    .defName))
                {
                    continue;
                }

                pawnKindDef.techHediffsChance =
                    ChoosePawnSettings_Mod.instance.Settings.CustomTechHediffsChances[pawnKindDef.defName];
                counter++;
            }

            if (counter > 0)
            {
                Main.LogMessage($"Set custom techhediffs for {counter} pawnkinds.");
            }
        }

        public static void ResetTechHediffsToVanillaRates()
        {
            foreach (var pawnKindDef in Main.AllPawnKinds)
            {
                pawnKindDef.techHediffsChance = VanillaTechHediffsChances[pawnKindDef.defName];
            }
        }
    }
}