using System.Collections.Generic;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

public static class GenerationAge
{
    public static readonly Dictionary<string, IntRange> VanillaGenerationAge = new();

    static GenerationAge()
    {
    }

    public static void Initialize()
    {
        saveVanillaGenerationAgeValues();
        setCustomGenerationAgeValues();
    }

    private static void saveVanillaGenerationAgeValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaGenerationAge[pawnKindDef.defName] =
                new IntRange(pawnKindDef.minGenerationAge, pawnKindDef.maxGenerationAge);
        }
    }

    private static void setCustomGenerationAgeValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomGenerationAge == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomGenerationAge.TryGetValue(pawnKindDef
                    .defName, out var value))
            {
                continue;
            }

            pawnKindDef.minGenerationAge = value.min;
            pawnKindDef.maxGenerationAge =
                ChoosePawnSettings_Mod.instance.Settings.CustomGenerationAge[pawnKindDef.defName].max;
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom GenerationAge for {counter} pawnkinds.");
        }
    }

    public static void ResetGenerationAgeToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.minGenerationAge = VanillaGenerationAge[pawnKindDef.defName].min;
            pawnKindDef.maxGenerationAge = VanillaGenerationAge[pawnKindDef.defName].max;
        }
    }
}