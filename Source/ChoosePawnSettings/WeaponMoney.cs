using System.Collections.Generic;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

public static class WeaponMoney
{
    public static readonly Dictionary<string, FloatRange> VanillaWeaponMoney =
        new Dictionary<string, FloatRange>();

    static WeaponMoney()
    {
    }

    public static void Initialize()
    {
        saveVanillaWeaponMoneyValues();
        setCustomWeaponMoneyValues();
    }

    private static void saveVanillaWeaponMoneyValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaWeaponMoney[pawnKindDef.defName] = pawnKindDef.weaponMoney;
        }
    }

    private static void setCustomWeaponMoneyValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomWeaponMoney == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomWeaponMoney.ContainsKey(pawnKindDef
                    .defName))
            {
                continue;
            }

            pawnKindDef.weaponMoney =
                ChoosePawnSettings_Mod.instance.Settings.CustomWeaponMoney[pawnKindDef.defName];
            counter++;
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom weaponMoney for {counter} pawnkinds.");
        }
    }

    public static void ResetWeaponMoneyToVanillaRates()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.weaponMoney = VanillaWeaponMoney[pawnKindDef.defName];
        }
    }
}