using System.Collections.Generic;
using ChoosePawnSettings.Settings;

namespace ChoosePawnSettings;

public static class CombatEnhancingDrugs
{
    public static readonly Dictionary<string, float> VanillaCombatEnhancingDrugsChances =
        new Dictionary<string, float>();

    static CombatEnhancingDrugs()
    {
    }

    public static void Initialize()
    {
        saveVanillaCombatEnhancingDrugsValues();
        setCustomCombatEnhancingDrugsValues();
    }

    private static void saveVanillaCombatEnhancingDrugsValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaCombatEnhancingDrugsChances[pawnKindDef.defName] = pawnKindDef.combatEnhancingDrugsChance;
        }
    }

    private static void setCustomCombatEnhancingDrugsValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomCombatEnhancingDrugsChances == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomCombatEnhancingDrugsChances.TryGetValue(pawnKindDef
                    .defName, out var chance))
            {
                continue;
            }

            pawnKindDef.combatEnhancingDrugsChance =
                chance;
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom combatenhancingdrugs for {counter} pawnkinds.");
        }
    }

    public static void ResetCombatEnhancingDrugsToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.combatEnhancingDrugsChance = VanillaCombatEnhancingDrugsChances[pawnKindDef.defName];
        }
    }
}