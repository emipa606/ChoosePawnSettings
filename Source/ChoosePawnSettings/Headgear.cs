using System.Collections.Generic;
using ChoosePawnSettings.Settings;

namespace ChoosePawnSettings;

public static class Headgear
{
    public static readonly Dictionary<string, float> VanillaHeadgearChances = new Dictionary<string, float>();

    static Headgear()
    {
    }

    public static void Initialize()
    {
        saveVanillaHeadgearValues();
        setCustomHeadgearValues();
    }

    private static void saveVanillaHeadgearValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaHeadgearChances[pawnKindDef.defName] = pawnKindDef.apparelAllowHeadgearChance;
        }
    }

    private static void setCustomHeadgearValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomHeadgearChances == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomHeadgearChances.ContainsKey(pawnKindDef.defName))
            {
                continue;
            }

            pawnKindDef.apparelAllowHeadgearChance =
                ChoosePawnSettings_Mod.instance.Settings.CustomHeadgearChances[pawnKindDef.defName];
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom headgearchance for {counter} pawnkinds.");
        }
    }

    public static void ResetHeadgearToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.apparelAllowHeadgearChance = VanillaHeadgearChances[pawnKindDef.defName];
        }
    }
}