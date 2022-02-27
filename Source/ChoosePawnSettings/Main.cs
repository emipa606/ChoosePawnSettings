using System.Collections.Generic;
using System.Linq;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

[StaticConstructorOnStartup]
public static class Main
{
    private static List<PawnKindDef> allPawnKinds;
    private static List<ThingDef> allWeapons;
    private static List<ThingDef> allApparel;

    static Main()
    {
        ChoosePawnSettings_Mod.instance.Settings.Initialize();
    }

    public static List<PawnKindDef> AllPawnKinds
    {
        get
        {
            if (allPawnKinds == null || allPawnKinds.Count == 0)
            {
                allPawnKinds = (from pawn in DefDatabase<PawnKindDef>.AllDefsListForReading
                    where pawn.RaceProps?.Humanlike == true
                    orderby pawn.label
                    select pawn).ToList();
            }

            return allPawnKinds;
        }
        set => allPawnKinds = value;
    }

    public static List<ThingDef> AllWeapons
    {
        get
        {
            if (allWeapons == null || allWeapons.Count == 0)
            {
                allWeapons = (from weapon in DefDatabase<ThingDef>.AllDefsListForReading
                    where weapon.IsWeapon
                    orderby weapon.label
                    select weapon).ToList();
            }

            return allWeapons;
        }
        set => allWeapons = value;
    }

    public static List<ThingDef> AllApparel
    {
        get
        {
            if (allApparel == null || allApparel.Count == 0)
            {
                allApparel = (from apparel in DefDatabase<ThingDef>.AllDefsListForReading
                    where apparel.IsApparel
                    orderby apparel.label
                    select apparel).ToList();
            }

            return allApparel;
        }
        set => allApparel = value;
    }


    public static void LogMessage(string message, bool forced = false, bool warning = false)
    {
        if (warning)
        {
            Log.Warning($"[ChoosePawnSettings]: {message}");
            return;
        }

        if (!forced && !ChoosePawnSettings_Mod.instance.Settings.VerboseLogging)
        {
            return;
        }

        Log.Message($"[ChoosePawnSettings]: {message}");
    }
}