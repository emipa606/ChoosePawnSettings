using System.Collections.Generic;
using System.Linq;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

public static class TechHediffTags
{
    public static readonly Dictionary<string, List<ThingDef>> TechHediffTagDictionary = new();

    public static readonly Dictionary<string, List<string>> VanillaTechHediffTagDictionary = new();

    static TechHediffTags()
    {
    }

    public static void Initialize()
    {
        saveVanillaTechHediffTagValues();
        setCustomTechHediffTagValues();
    }

    private static void saveVanillaTechHediffTagValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            VanillaTechHediffTagDictionary[pawnKindDef.defName] = pawnKindDef.techHediffsTags.ListFullCopyOrNull();
        }

        foreach (var techHediff in Main.AllTechHediffs)
        {
            if (techHediff.techHediffsTags == null || !techHediff.techHediffsTags.Any())
            {
                continue;
            }

            foreach (var techHediffTag in techHediff.techHediffsTags)
            {
                if (!TechHediffTagDictionary.TryGetValue(techHediffTag, out var value))
                {
                    TechHediffTagDictionary[techHediffTag] = [techHediff];
                    continue;
                }

                value.Add(techHediff);
            }
        }
    }

    private static void setCustomTechHediffTagValues()
    {
        if (ChoosePawnSettings_Mod.instance?.Settings?.CustomTechHediffTags == null)
        {
            return;
        }

        var counter = 0;
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            if (!ChoosePawnSettings_Mod.instance.Settings.CustomTechHediffTags.ContainsKey(pawnKindDef.defName))
            {
                continue;
            }

            counter++;
            if (ChoosePawnSettings_Mod.instance.Settings.CustomTechHediffTags[pawnKindDef.defName] == string.Empty)
            {
                pawnKindDef.techHediffsTags = [];
                continue;
            }

            pawnKindDef.techHediffsTags =
                ChoosePawnSettings_Mod.instance.Settings.CustomTechHediffTags[pawnKindDef.defName].Split('|').ToList();
        }

        if (counter > 0)
        {
            Main.LogMessage($"Set custom techHediffTags for {counter} pawnkinds.");
        }
    }


    public static void ResetTechHediffTagsToVanillaValues()
    {
        foreach (var pawnKindDef in Main.AllPawnKinds)
        {
            pawnKindDef.techHediffsTags = VanillaTechHediffTagDictionary[pawnKindDef.defName].ListFullCopyOrNull();

            ChoosePawnSettings_Mod.instance.Settings.CustomTechHediffTags?.Remove(pawnKindDef.defName);
        }
    }

    public static void ResetTechHediffTagsToVanillaValues(string pawnKindDefName)
    {
        Main.LogMessage(
            $"Resetting {pawnKindDefName} to vanilla techHediff tags");
        PawnKindDef.Named(pawnKindDefName).techHediffsTags =
            VanillaTechHediffTagDictionary[pawnKindDefName].ListFullCopyOrNull();
        ChoosePawnSettings_Mod.instance.Settings.CustomTechHediffTags?.Remove(pawnKindDefName);
    }
}