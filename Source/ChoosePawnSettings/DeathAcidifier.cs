using System.Collections.Generic;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

public static class DeathAcidifier
{
    public static readonly Dictionary<string, bool> VanillaDeathAcidifiers = new Dictionary<string, bool>();

    public static ThingDef DeathAcidifierThingDef;

    static DeathAcidifier()
    {
    }

    public static void Initialize()
    {
        DeathAcidifierThingDef = ThingDef.Named("DeathAcidifier");
        saveVanillaDeathAcidifierValues();
        setCustomDeathAcidifierValues();
    }

    private static void saveVanillaDeathAcidifierValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaDeathAcidifiers[pawnKindDef.defName] =
                pawnKindDef.techHediffsRequired?.Contains(DeathAcidifierThingDef) == true;
        }
    }

    private static void setCustomDeathAcidifierValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomDeathAcidifier == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomDeathAcidifier.TryGetValue(pawnKindDef.defName,
                    out var value))
            {
                continue;
            }

            if (value)
            {
                if (pawnKindDef.techHediffsRequired == null)
                {
                    pawnKindDef.techHediffsRequired = [];
                }

                if (!pawnKindDef.techHediffsRequired.Contains(DeathAcidifierThingDef))
                {
                    pawnKindDef.techHediffsRequired.Add(DeathAcidifierThingDef);
                }
            }
            else
            {
                if (pawnKindDef.techHediffsRequired?.Contains(DeathAcidifierThingDef) == true)
                {
                    pawnKindDef.techHediffsRequired.Remove(DeathAcidifierThingDef);
                }
            }

            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Changed death acidifiers for {counter} pawnkinds.");
        }
    }

    public static void ResetDeathAcidifierToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (VanillaDeathAcidifiers[pawnKindDef.defName])
            {
                if (pawnKindDef.techHediffsRequired == null)
                {
                    pawnKindDef.techHediffsRequired = [];
                }

                if (!pawnKindDef.techHediffsRequired.Contains(DeathAcidifierThingDef))
                {
                    pawnKindDef.techHediffsRequired.Add(DeathAcidifierThingDef);
                }
            }
            else
            {
                if (pawnKindDef.techHediffsRequired?.Contains(DeathAcidifierThingDef) == true)
                {
                    pawnKindDef.techHediffsRequired.Remove(DeathAcidifierThingDef);
                }
            }
        }
    }
}