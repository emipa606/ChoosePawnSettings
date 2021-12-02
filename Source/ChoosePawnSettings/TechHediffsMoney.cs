using System.Collections.Generic;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

public static class TechHediffsMoney
{
    public static readonly Dictionary<string, FloatRange> VanillaTechHediffsMoney =
        new Dictionary<string, FloatRange>();

    static TechHediffsMoney()
    {
    }

    public static void Initialize()
    {
        saveVanillaTechHediffsMoneyValues();
        setCustomTechHediffsMoneyValues();
    }

    private static void saveVanillaTechHediffsMoneyValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaTechHediffsMoney[pawnKindDef.defName] = pawnKindDef.techHediffsMoney;
        }
    }

    private static void setCustomTechHediffsMoneyValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomTechHediffsMoney == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomTechHediffsMoney.ContainsKey(pawnKindDef
                    .defName))
            {
                continue;
            }

            pawnKindDef.techHediffsMoney =
                ChoosePawnSettings_Mod.instance.Settings.CustomTechHediffsMoney[pawnKindDef.defName];
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom techhediffsmoney for {counter} pawnkinds.");
        }
    }

    public static void ResetTechHediffsMoneyToVanillaRates()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.techHediffsMoney = VanillaTechHediffsMoney[pawnKindDef.defName];
        }
    }
}