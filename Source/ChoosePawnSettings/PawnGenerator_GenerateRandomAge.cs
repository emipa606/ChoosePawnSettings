using ChoosePawnSettings.Settings;
using HarmonyLib;
using Verse;

namespace ChoosePawnSettings;

[HarmonyPatch(typeof(PawnGenerator), "GenerateRandomAge")]
[HarmonyBefore("rimworld.erdelf.alien_race.main")]
public static class PawnGenerator_GenerateRandomAge
{
    public static void Prefix(ref Pawn pawn, PawnGenerationRequest request)
    {
        if (request.FixedGender != null || pawn.kindDef.fixedGender != null || !pawn.RaceProps.hasGenders)
        {
            return;
        }

        if (!ChoosePawnSettings_Mod.instance.Settings.CustomGenderProbabilities.ContainsKey(pawn.kindDef.defName))
        {
            return;
        }

        pawn.gender =
            Rand.Chance(ChoosePawnSettings_Mod.instance.Settings.CustomGenderProbabilities[pawn.kindDef.defName])
                ? Gender.Female
                : Gender.Male;
    }
}