using System.Collections.Generic;
using System.Linq;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

public static class ApparelTags
{
    public static readonly Dictionary<string, List<ThingDef>> ApparelTagDictionary = new();

    public static readonly Dictionary<string, List<string>> VanillaApparelTagsDictionary = new();

    static ApparelTags()
    {
    }

    public static void Initialize()
    {
        saveVanillaApparelValues();
        setCustomApparelTagValues();
    }

    private static void saveVanillaApparelValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaApparelTagsDictionary[pawnKindDef.defName] = pawnKindDef.apparelTags.ListFullCopyOrNull();
        }

        foreach (var apparel in Main.AllApparel)
        {
            if (apparel.apparel?.tags == null || !apparel.apparel.tags.Any())
            {
                continue;
            }

            foreach (var apparelTag in apparel.apparel.tags)
            {
                if (!ApparelTagDictionary.TryGetValue(apparelTag, out var value))
                {
                    ApparelTagDictionary[apparelTag] = [apparel];
                    continue;
                }

                value.Add(apparel);
            }
        }
    }

    private static void setCustomApparelTagValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomApparelTags == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomApparelTags.ContainsKey(pawnKindDef.defName))
            {
                continue;
            }

            counter++;
            if (ChoosePawnSettings_Mod.instance.Settings.CustomApparelTags[pawnKindDef.defName] == string.Empty)
            {
                pawnKindDef.apparelTags = [];
                continue;
            }

            pawnKindDef.apparelTags =
                ChoosePawnSettings_Mod.instance.Settings.CustomApparelTags[pawnKindDef.defName].Split('|').ToList();
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom apparelTags for {counter} pawnkinds.");
        }
    }


    public static void ResetApparelTagsToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.apparelTags = VanillaApparelTagsDictionary[pawnKindDef.defName].ListFullCopyOrNull();
            ChoosePawnSettings_Mod.instance.Settings.CustomApparelTags?.Remove(pawnKindDef.defName);
        }
    }

    public static void ResetApparelTagsToVanillaValues(string pawnKindDefName)
    {
        Main.LogMessage(
            $"Resetting {pawnKindDefName} to vanilla apparel tags");
        PawnKindDef.Named(pawnKindDefName).apparelTags =
            VanillaApparelTagsDictionary[pawnKindDefName].ListFullCopyOrNull();
        ChoosePawnSettings_Mod.instance.Settings.CustomApparelTags?.Remove(pawnKindDefName);
    }
}