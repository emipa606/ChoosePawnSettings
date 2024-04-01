using System.Collections.Generic;
using ChoosePawnSettings.Settings;

namespace ChoosePawnSettings;

public static class CombatPower
{
    public static readonly Dictionary<string, float> VanillaCombatPowers = new Dictionary<string, float>();

    static CombatPower()
    {
    }

    public static void Initialize()
    {
        saveVanillaCombatPowerValues();
        setCustomCombatPowerValues();
    }

    private static void saveVanillaCombatPowerValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaCombatPowers[pawnKindDef.defName] = pawnKindDef.combatPower;
        }
    }

    private static void setCustomCombatPowerValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomCombatPowers == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomCombatPowers.TryGetValue(pawnKindDef.defName,
                    out var power))
            {
                continue;
            }

            pawnKindDef.combatPower = power;
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom Combat Power for {counter} pawnkinds.");
        }
    }

    public static void ResetCombatPowerToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.combatPower = VanillaCombatPowers[pawnKindDef.defName];
        }
    }
}