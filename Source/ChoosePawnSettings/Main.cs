using System.Collections.Generic;
using System.Linq;
using ChoosePawnSettings.Settings;
using Verse;

namespace ChoosePawnSettings;

[StaticConstructorOnStartup]
public static class Main
{
    private static List<PawnKindDef> allPawnKinds;

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