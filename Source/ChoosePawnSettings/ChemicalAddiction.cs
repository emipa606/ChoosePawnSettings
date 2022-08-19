using System.Collections.Generic;
using ChoosePawnSettings.Settings;

namespace ChoosePawnSettings;

public static class ChemicalAddiction
{
    public static readonly Dictionary<string, float> VanillaChemicalAddictionChances =
        new Dictionary<string, float>();

    static ChemicalAddiction()
    {
    }

    public static void Initialize()
    {
        saveVanillaChemicalAddictionValues();
        setCustomChemicalAddictiongValues();
    }

    private static void saveVanillaChemicalAddictionValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaChemicalAddictionChances[pawnKindDef.defName] = pawnKindDef.chemicalAddictionChance;
        }
    }

    private static void setCustomChemicalAddictiongValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomChemicalAddictionChances == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomChemicalAddictionChances.ContainsKey(pawnKindDef
                    .defName))
            {
                continue;
            }

            pawnKindDef.chemicalAddictionChance =
                ChoosePawnSettings_Mod.instance.Settings.CustomChemicalAddictionChances[pawnKindDef.defName];
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom chemicaladdiction for {counter} pawnkinds.");
        }
    }

    public static void ResetChemicalAddictionToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.chemicalAddictionChance = VanillaChemicalAddictionChances[pawnKindDef.defName];
        }
    }
}