using System.Collections.Generic;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

public static class ApparelMoney
{
    public static readonly Dictionary<string, FloatRange> VanillaApparelMoney = new();

    static ApparelMoney()
    {
    }

    public static void Initialize()
    {
        saveVanillaApparelMoneyValues();
        setCustomApparelMoneyValues();
    }

    private static void saveVanillaApparelMoneyValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaApparelMoney[pawnKindDef.defName] = pawnKindDef.apparelMoney;
        }
    }

    private static void setCustomApparelMoneyValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomApparelMoney == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomApparelMoney.TryGetValue(pawnKindDef.defName,
                    out var value))
            {
                continue;
            }

            pawnKindDef.apparelMoney = value;
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom apparelMoney for {counter} pawnkinds.");
        }
    }

    public static void ResetApparelMoneyToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.apparelMoney = VanillaApparelMoney[pawnKindDef.defName];
        }
    }
}