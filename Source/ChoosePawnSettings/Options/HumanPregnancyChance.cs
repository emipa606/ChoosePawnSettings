using System.Collections.Generic;
using ChoosePawnSettings.Settings;

namespace ChoosePawnSettings;

public static class HumanPregnancyChance
{
    public static readonly Dictionary<string, float> VanillaHumanPregnancyChanceChances = new();

    static HumanPregnancyChance()
    {
    }

    public static void Initialize()
    {
        saveVanillaHumanPregnancyChanceValues();
        setCustomHumanPregnancyChanceValues();
    }

    private static void saveVanillaHumanPregnancyChanceValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaHumanPregnancyChanceChances[pawnKindDef.defName] = pawnKindDef.humanPregnancyChance;
        }
    }

    private static void setCustomHumanPregnancyChanceValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.HumanPregnancyChanceChances == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.HumanPregnancyChanceChances.TryGetValue(pawnKindDef
                    .defName, out var chance))
            {
                continue;
            }

            pawnKindDef.humanPregnancyChance = chance;
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom humanPregnancyChance for {counter} pawnkinds.");
        }
    }

    public static void ResetHumanPregnancyChanceToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.humanPregnancyChance = VanillaHumanPregnancyChanceChances[pawnKindDef.defName];
        }
    }
}