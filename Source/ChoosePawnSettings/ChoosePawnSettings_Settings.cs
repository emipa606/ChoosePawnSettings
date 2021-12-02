using System.Collections.Generic;
using Verse;

namespace ChoosePawnSettings;

public class ChoosePawnSettings_Settings : ModSettings
{
    public Dictionary<string, float> CustomBiocodeChances =
        new Dictionary<string, float>();

    private List<string> customBiocodeChancesKeys;

    private List<float> customBiocodeChancesValues;

    public Dictionary<string, float> CustomChemicalAddictionChances =
        new Dictionary<string, float>();

    private List<string> customChemicalAddictionChancesKeys;

    private List<float> customChemicalAddictionChancesValues;

    public Dictionary<string, float> CustomCombatEnhancingDrugsChances =
        new Dictionary<string, float>();

    private List<string> customCombatEnhancingDrugsChancesKeys;

    private List<float> customCombatEnhancingDrugsChancesValues;

    public Dictionary<string, float> CustomTechHediffsChances =
        new Dictionary<string, float>();

    private List<string> customTechHediffsChancesKeys;

    private List<float> customTechHediffsChancesValues;

    public Dictionary<string, FloatRange> CustomTechHediffsMoney =
        new Dictionary<string, FloatRange>();

    private List<string> customTechHediffsMoneyKeys;

    private List<FloatRange> customTechHediffsMoneyValues;

    public bool VerboseLogging;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
        Scribe_Collections.Look(ref CustomBiocodeChances, "CustomBiocodeChances", LookMode.Value,
            LookMode.Value,
            ref customBiocodeChancesKeys, ref customBiocodeChancesValues);
        Scribe_Collections.Look(ref CustomChemicalAddictionChances, "CustomChemicalAddictionChances",
            LookMode.Value,
            LookMode.Value,
            ref customChemicalAddictionChancesKeys, ref customChemicalAddictionChancesValues);
        Scribe_Collections.Look(ref CustomCombatEnhancingDrugsChances, "CustomCombatEnhancingDrugsChances",
            LookMode.Value,
            LookMode.Value,
            ref customCombatEnhancingDrugsChancesKeys, ref customCombatEnhancingDrugsChancesValues);
        Scribe_Collections.Look(ref CustomTechHediffsChances, "CustomTechHediffsChances",
            LookMode.Value,
            LookMode.Value,
            ref customTechHediffsChancesKeys, ref customTechHediffsChancesValues);
        Scribe_Collections.Look(ref CustomTechHediffsMoney, "CustomTechHediffsMoney",
            LookMode.Value,
            LookMode.Value,
            ref customTechHediffsMoneyKeys, ref customTechHediffsMoneyValues);
    }

    public void ResetBiocodeValues()
    {
        customBiocodeChancesKeys = new List<string>();
        customBiocodeChancesValues = new List<float>();
        CustomBiocodeChances = new Dictionary<string, float>();
        Biocoding.ResetBiocodingToVanillaRates();
    }

    public void ResetChemicalAddictionValues()
    {
        customChemicalAddictionChancesKeys = new List<string>();
        customChemicalAddictionChancesValues = new List<float>();
        CustomChemicalAddictionChances = new Dictionary<string, float>();
        ChemicalAddiction.ResetChemicalAddictionToVanillaRates();
    }

    public void ResetCombatEnhancingDrugsValues()
    {
        customCombatEnhancingDrugsChancesKeys = new List<string>();
        customCombatEnhancingDrugsChancesValues = new List<float>();
        CustomCombatEnhancingDrugsChances = new Dictionary<string, float>();
        CombatEnhancingDrugs.ResetCombatEnhancingDrugsToVanillaRates();
    }

    public void ResetTechHediffsValues()
    {
        customTechHediffsChancesKeys = new List<string>();
        customTechHediffsChancesValues = new List<float>();
        CustomTechHediffsChances = new Dictionary<string, float>();
        TechHediffs.ResetTechHediffsToVanillaRates();
    }

    public void ResetTechHediffsMoneyValues()
    {
        customTechHediffsMoneyKeys = new List<string>();
        customTechHediffsMoneyValues = new List<FloatRange>();
        CustomTechHediffsMoney = new Dictionary<string, FloatRange>();
        TechHediffsMoney.ResetTechHediffsMoneyToVanillaRates();
    }

    public void ResetManualValues()
    {
        ResetBiocodeValues();
        ResetChemicalAddictionValues();
        ResetCombatEnhancingDrugsValues();
        ResetTechHediffsValues();
        ResetTechHediffsMoneyValues();
    }
}