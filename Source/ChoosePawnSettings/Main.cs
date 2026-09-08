using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ChoosePawnSettings.Settings;
using HarmonyLib;
using Verse;

namespace ChoosePawnSettings;

[StaticConstructorOnStartup]
public static class Main
{
    static Main()
    {
        ChoosePawnSettings_Settings.Initialize();
        new Harmony("Mlie.ChoosePawnSettings").PatchAll(Assembly.GetExecutingAssembly());
    }

    public static List<PawnKindDef> AllPawnKinds
    {
        get
        {
            if (field == null || field.Count == 0)
            {
                field = (from pawn in DefDatabase<PawnKindDef>.AllDefsListForReading
                    where pawn.RaceProps?.Humanlike == true && !pawn.race.IsCorpse
                    orderby pawn.label
                    select pawn).ToList();
            }

            return field;
        }
    }

    public static List<ThingDef> AllWeapons
    {
        get
        {
            if (field == null || field.Count == 0)
            {
                field = (from weapon in DefDatabase<ThingDef>.AllDefsListForReading
                    where weapon.IsWeapon
                    orderby weapon.label
                    select weapon).ToList();
            }

            return field;
        }
    }

    public static List<ThingDef> AllTechHediffs
    {
        get
        {
            if (field == null || field.Count == 0)
            {
                field = (from techHediff in DefDatabase<ThingDef>.AllDefsListForReading
                    where techHediff.isTechHediff
                    orderby techHediff.label
                    select techHediff).ToList();
            }

            return field;
        }
    }

    public static List<ThingDef> AllApparel
    {
        get
        {
            if (field == null || field.Count == 0)
            {
                field = (from apparel in DefDatabase<ThingDef>.AllDefsListForReading
                    where apparel.IsApparel
                    orderby apparel.label
                    select apparel).ToList();
            }

            return field;
        }
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