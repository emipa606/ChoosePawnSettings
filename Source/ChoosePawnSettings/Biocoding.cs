using System.Collections.Generic;
using ChoosePawnSettings.Settings;

namespace ChoosePawnSettings;

public static class Biocoding
{
    public static readonly Dictionary<string, float> VanillaBiocodeChances = new Dictionary<string, float>();

    static Biocoding()
    {
    }

    public static void Initialize()
    {
        saveVanillaBiocodingValues();
        setCustomBiocodingValues();
    }

    private static void saveVanillaBiocodingValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaBiocodeChances[pawnKindDef.defName] = pawnKindDef.biocodeWeaponChance;
        }
    }

    private static void setCustomBiocodingValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomBiocodeChances == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomBiocodeChances.TryGetValue(pawnKindDef.defName,
                    out var chance))
            {
                continue;
            }

            pawnKindDef.biocodeWeaponChance = chance;
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom biocodechance for {counter} pawnkinds.");
        }
    }

    public static void ResetBiocodingToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.biocodeWeaponChance = VanillaBiocodeChances[pawnKindDef.defName];
        }
    }
}