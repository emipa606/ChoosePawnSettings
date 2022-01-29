using System.Collections.Generic;
using ChoosePawnSettings.Settings;

namespace ChoosePawnSettings;

public static class RoyalTitleChance
{
    public static readonly Dictionary<string, float> VanillaRoyalTitleChances = new Dictionary<string, float>();

    static RoyalTitleChance()
    {
    }

    public static void Initialize()
    {
        saveVanillaRoyalTitleChances();
        setCustomRoyalTitleChances();
    }

    private static void saveVanillaRoyalTitleChances()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaRoyalTitleChances[pawnKindDef.defName] = pawnKindDef.royalTitleChance;
        }
    }

    private static void setCustomRoyalTitleChances()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomRoyalTitleChances == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomRoyalTitleChances.ContainsKey(pawnKindDef.defName))
            {
                continue;
            }

            pawnKindDef.royalTitleChance =
                ChoosePawnSettings_Mod.instance.Settings.CustomRoyalTitleChances[pawnKindDef.defName];
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom royalTitleChance for {counter} pawnkinds.");
        }
    }

    public static void ResetRoyalTitleChanceToVanillaRates()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.royalTitleChance = VanillaRoyalTitleChances[pawnKindDef.defName];
        }
    }
}