using System.Collections.Generic;
using System.Linq;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

public static class WeaponTags
{
    public static readonly Dictionary<string, List<ThingDef>> WeaponTagDictionary =
        new Dictionary<string, List<ThingDef>>();

    public static readonly Dictionary<string, List<string>> VanillaWeaponTagsDictionary =
        new Dictionary<string, List<string>>();

    static WeaponTags()
    {
    }

    public static void Initialize()
    {
        saveVanillaWeaponValues();
        setCustomWeaponTagValues();
    }

    private static void saveVanillaWeaponValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaWeaponTagsDictionary[pawnKindDef.defName] = pawnKindDef.weaponTags.ListFullCopyOrNull();
        }

        foreach (var weapon in Main.AllWeapons)
        {
            if (weapon.weaponTags == null || !weapon.weaponTags.Any())
            {
                continue;
            }

            foreach (var weaponTag in weapon.weaponTags)
            {
                if (!WeaponTagDictionary.TryGetValue(weaponTag, out var value))
                {
                    WeaponTagDictionary[weaponTag] = [weapon];
                    continue;
                }

                value.Add(weapon);
            }
        }
    }

    private static void setCustomWeaponTagValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomWeaponTags == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomWeaponTags.ContainsKey(pawnKindDef.defName))
            {
                continue;
            }

            counter++;
            if (ChoosePawnSettings_Mod.instance.Settings.CustomWeaponTags[pawnKindDef.defName] == string.Empty)
            {
                pawnKindDef.weaponTags = [];
                continue;
            }

            pawnKindDef.weaponTags =
                ChoosePawnSettings_Mod.instance.Settings.CustomWeaponTags[pawnKindDef.defName].Split('|').ToList();
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom weaponTags for {counter} pawnkinds.");
        }
    }


    public static void ResetWeaponTagsToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.weaponTags = VanillaWeaponTagsDictionary[pawnKindDef.defName].ListFullCopyOrNull();

            if (ChoosePawnSettings_Mod.instance.Settings.CustomWeaponTags?.ContainsKey(pawnKindDef.defName) == true)
            {
                ChoosePawnSettings_Mod.instance.Settings.CustomWeaponTags.Remove(pawnKindDef.defName);
            }
        }
    }

    public static void ResetWeaponTagsToVanillaValues(string pawnKindDefName)
    {
        Main.LogMessage(
            $"Resetting {pawnKindDefName} to vanilla weapon tags");
        PawnKindDef.Named(pawnKindDefName).weaponTags =
            VanillaWeaponTagsDictionary[pawnKindDefName].ListFullCopyOrNull();
        if (ChoosePawnSettings_Mod.instance.Settings.CustomWeaponTags?.ContainsKey(pawnKindDefName) == true)
        {
            ChoosePawnSettings_Mod.instance.Settings.CustomWeaponTags.Remove(pawnKindDefName);
        }
    }
}