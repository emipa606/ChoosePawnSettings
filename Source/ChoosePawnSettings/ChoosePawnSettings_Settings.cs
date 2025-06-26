using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ChoosePawnSettings;

public class ChoosePawnSettings_Settings : ModSettings
{
    public Dictionary<string, FloatRange> CustomApparelMoney = new();

    private List<string> customApparelMoneyKeys;

    private List<FloatRange> customApparelMoneyValues;
    private List<string> customApparelTagKeys;
    public Dictionary<string, string> CustomApparelTags;

    private List<string> customApparelTagValues;

    public Dictionary<string, float> CustomBiocodeChances = new();

    private List<string> customBiocodeChancesKeys;

    private List<float> customBiocodeChancesValues;

    public Dictionary<string, float> CustomChemicalAddictionChances = new();

    private List<string> customChemicalAddictionChancesKeys;

    private List<float> customChemicalAddictionChancesValues;

    public Dictionary<string, float> CustomCombatEnhancingDrugsChances = new();

    private List<string> customCombatEnhancingDrugsChancesKeys;

    private List<float> customCombatEnhancingDrugsChancesValues;

    public Dictionary<string, float> CustomCombatPowers = new();

    private List<string> customCombatPowersKeys;

    private List<float> customCombatPowersValues;

    public Dictionary<string, bool> CustomDeathAcidifier;
    public List<string> customDeathAcidifiersKeys;
    public List<bool> customDeathAcidifiersValues;

    public Dictionary<string, float> CustomGenderProbabilities = new();

    private List<string> customGenderProbabilitiesKeys;

    private List<float> customGenderProbabilitiesValues;

    public Dictionary<string, IntRange> CustomGenerationAge = new();

    private List<string> customGenerationAgeKeys;

    private List<IntRange> customGenerationAgeValues;

    public Dictionary<string, float> CustomHeadgearChances = new();

    private List<string> customHeadgearChancesKeys;

    private List<float> customHeadgearChancesValues;

    public Dictionary<string, float> CustomRoyalTitleChances = new();

    private List<string> customRoyalTitleChancesKeys;

    private List<float> customRoyalTitleChancesValues;

    public Dictionary<string, float> CustomTechHediffsChances = new();

    private List<string> customTechHediffsChancesKeys;

    private List<float> customTechHediffsChancesValues;

    public Dictionary<string, FloatRange> CustomTechHediffsMoney = new();

    private List<string> customTechHediffsMoneyKeys;

    private List<FloatRange> customTechHediffsMoneyValues;

    private List<string> customTechHediffTagKeys;
    public Dictionary<string, string> CustomTechHediffTags;

    private List<string> customTechHediffTagValues;

    public Dictionary<string, FloatRange> CustomWeaponMoney = new();

    private List<string> customWeaponMoneyKeys;

    private List<FloatRange> customWeaponMoneyValues;
    private List<string> customWeaponTagKeys;

    public Dictionary<string, string> CustomWeaponTags;

    private List<string> customWeaponTagValues;
    public bool VerboseLogging;


    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref VerboseLogging, "VerboseLogging");
        Scribe_Collections.Look(ref CustomBiocodeChances, "CustomBiocodeChances", LookMode.Value,
            LookMode.Value,
            ref customBiocodeChancesKeys, ref customBiocodeChancesValues);
        Scribe_Collections.Look(ref CustomHeadgearChances, "CustomHeadgearChances", LookMode.Value,
            LookMode.Value,
            ref customHeadgearChancesKeys, ref customHeadgearChancesValues);
        Scribe_Collections.Look(ref CustomCombatPowers, "CustomCombatPowers", LookMode.Value,
            LookMode.Value,
            ref customCombatPowersKeys, ref customCombatPowersValues);
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
        Scribe_Collections.Look(ref CustomWeaponMoney, "CustomWeaponMoney",
            LookMode.Value,
            LookMode.Value,
            ref customWeaponMoneyKeys, ref customWeaponMoneyValues);
        Scribe_Collections.Look(ref CustomGenerationAge, "CustomGenerationAge",
            LookMode.Value,
            LookMode.Value,
            ref customGenerationAgeKeys, ref customGenerationAgeValues);
        Scribe_Collections.Look(ref CustomApparelMoney, "CustomApparelMoney",
            LookMode.Value,
            LookMode.Value,
            ref customApparelMoneyKeys, ref customApparelMoneyValues);
        Scribe_Collections.Look(ref CustomRoyalTitleChances, "CustomRoyalTitleChances",
            LookMode.Value,
            LookMode.Value,
            ref customRoyalTitleChancesKeys, ref customRoyalTitleChancesValues);
        Scribe_Collections.Look(ref CustomWeaponTags, "CustomWeaponTags",
            LookMode.Value,
            LookMode.Value,
            ref customWeaponTagKeys, ref customWeaponTagValues);
        Scribe_Collections.Look(ref CustomApparelTags, "CustomApparelTags",
            LookMode.Value,
            LookMode.Value,
            ref customApparelTagKeys, ref customApparelTagValues);
        Scribe_Collections.Look(ref CustomTechHediffTags, "CustomTechHediffTags",
            LookMode.Value,
            LookMode.Value,
            ref customTechHediffTagKeys, ref customTechHediffTagValues);
        Scribe_Collections.Look(ref CustomDeathAcidifier, "CustomDeathAcidifier",
            LookMode.Value,
            LookMode.Value,
            ref customDeathAcidifiersKeys, ref customDeathAcidifiersValues);
        Scribe_Collections.Look(ref CustomGenderProbabilities, "CustomGenderProbabilities",
            LookMode.Value,
            LookMode.Value,
            ref customGenderProbabilitiesKeys, ref customGenderProbabilitiesValues);
    }

    public static void Initialize()
    {
        Biocoding.Initialize();
        ChemicalAddiction.Initialize();
        CombatEnhancingDrugs.Initialize();
        Headgear.Initialize();
        CombatPower.Initialize();
        TechHediffs.Initialize();
        TechHediffsMoney.Initialize();
        WeaponMoney.Initialize();
        ApparelMoney.Initialize();
        WeaponTags.Initialize();
        ApparelTags.Initialize();
        TechHediffTags.Initialize();
        DeathAcidifier.Initialize();
        GenerationAge.Initialize();
        if (ModLister.RoyaltyInstalled)
        {
            RoyalTitleChance.Initialize();
        }
    }

    public void ResetValues(string valueLabel)
    {
        if (valueLabel is "biocoding" or "all")
        {
            customBiocodeChancesKeys = [];
            customBiocodeChancesValues = [];
            CustomBiocodeChances = new Dictionary<string, float>();
            Biocoding.ResetBiocodingToVanillaValues();
        }

        if (valueLabel is "chemicaladdiction" or "all")
        {
            customChemicalAddictionChancesKeys = [];
            customChemicalAddictionChancesValues = [];
            CustomChemicalAddictionChances = new Dictionary<string, float>();
            ChemicalAddiction.ResetChemicalAddictionToVanillaValues();
        }

        if (valueLabel is "combatenhancingdrugs" or "all")
        {
            customCombatEnhancingDrugsChancesKeys = [];
            customCombatEnhancingDrugsChancesValues = [];
            CustomCombatEnhancingDrugsChances = new Dictionary<string, float>();
            CombatEnhancingDrugs.ResetCombatEnhancingDrugsToVanillaValues();
        }

        if (valueLabel is "headgear" or "all")
        {
            customHeadgearChancesKeys = [];
            customHeadgearChancesValues = [];
            CustomHeadgearChances = new Dictionary<string, float>();
            Headgear.ResetHeadgearToVanillaValues();
        }

        if (valueLabel is "combatpower" or "all")
        {
            customCombatPowersKeys = [];
            customCombatPowersValues = [];
            CustomCombatPowers = new Dictionary<string, float>();
            CombatPower.ResetCombatPowerToVanillaValues();
        }

        if (valueLabel is "techhediffs" or "all")
        {
            customTechHediffsChancesKeys = [];
            customTechHediffsChancesValues = [];
            CustomTechHediffsChances = new Dictionary<string, float>();
            TechHediffs.ResetTechHediffsToVanillaValues();
        }

        if (valueLabel is "techhediffsmoney" or "all")
        {
            customTechHediffsMoneyKeys = [];
            customTechHediffsMoneyValues = [];
            CustomTechHediffsMoney = new Dictionary<string, FloatRange>();
            TechHediffsMoney.ResetTechHediffsMoneyToVanillaValues();
        }

        if (valueLabel is "techhedifftags" or "all")
        {
            customTechHediffTagKeys = [];
            customTechHediffTagValues = [];
            CustomTechHediffTags = new Dictionary<string, string>();
            TechHediffTags.ResetTechHediffTagsToVanillaValues();
        }

        if (valueLabel is "weaponmoney" or "all")
        {
            customWeaponMoneyKeys = [];
            customWeaponMoneyValues = [];
            CustomWeaponMoney = new Dictionary<string, FloatRange>();
            WeaponMoney.ResetWeaponMoneyToVanillaValues();
        }

        if (valueLabel is "apparelmoney" or "all")
        {
            customApparelMoneyKeys = [];
            customApparelMoneyValues = [];
            CustomApparelMoney = new Dictionary<string, FloatRange>();
            ApparelMoney.ResetApparelMoneyToVanillaValues();
        }

        if (valueLabel is "weapontags" or "all")
        {
            customWeaponTagKeys = [];
            customWeaponTagValues = [];
            CustomWeaponTags = new Dictionary<string, string>();
            WeaponTags.ResetWeaponTagsToVanillaValues();
        }

        if (valueLabel is "appareltags" or "all")
        {
            customApparelTagKeys = [];
            customApparelTagValues = [];
            CustomApparelTags = new Dictionary<string, string>();
            ApparelTags.ResetApparelTagsToVanillaValues();
        }

        if (valueLabel is "deathacidifier" or "all")
        {
            customDeathAcidifiersKeys = [];
            customDeathAcidifiersValues = [];
            CustomDeathAcidifier = new Dictionary<string, bool>();
            DeathAcidifier.ResetDeathAcidifierToVanillaValues();
        }

        if (valueLabel is "generationage" or "all")
        {
            customGenerationAgeKeys = [];
            customGenerationAgeValues = [];
            CustomGenerationAge = new Dictionary<string, IntRange>();
            GenerationAge.ResetGenerationAgeToVanillaValues();
        }

        if (valueLabel is "royaltitlechance" or "all")
        {
            customRoyalTitleChancesKeys = [];
            customRoyalTitleChancesValues = [];
            CustomRoyalTitleChances = new Dictionary<string, float>();
            RoyalTitleChance.ResetRoyalTitleChanceToVanillaRates();
        }

        if (valueLabel is not ("genderprobabilities" or "all"))
        {
            return;
        }

        customGenderProbabilitiesKeys = [];
        customGenderProbabilitiesValues = [];
        CustomGenderProbabilities = new Dictionary<string, float>();
    }

    public bool HasCustomValues(string type = null)
    {
        switch (type)
        {
            case null or "biocoding" when CustomBiocodeChances?.Any() == true:
            case null or "chemicaladdiction" when CustomChemicalAddictionChances?.Any() == true:
            case null or "combatenhancingdrugs" when CustomCombatEnhancingDrugsChances?.Any() == true:
            case null or "headgear" when CustomHeadgearChances?.Any() == true:
            case null or "combatpower" when CustomCombatPowers?.Any() == true:
            case null or "techhediffs" when CustomTechHediffsChances?.Any() == true:
            case null or "techhediffsmoney" when CustomTechHediffsMoney?.Any() == true:
            case null or "weaponmoney" when CustomWeaponMoney?.Any() == true:
            case null or "apparelmoney" when CustomApparelMoney?.Any() == true:
            case null or "weapontags" when CustomWeaponTags?.Any() == true:
            case null or "appareltags" when CustomApparelTags?.Any() == true:
            case null or "techhedifftags" when CustomTechHediffTags?.Any() == true:
            case null or "deathacidifier" when CustomDeathAcidifier?.Any() == true:
            case null or "generationage" when CustomGenerationAge?.Any() == true:
            case null or "royaltitlechance" when CustomRoyalTitleChances?.Any() == true:
            case null or "genderprobabilities" when CustomGenderProbabilities?.Any() == true:
                return true;
            default:
                return false;
        }
    }
}