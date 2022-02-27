using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ChoosePawnSettings;

public class ChoosePawnSettings_Settings : ModSettings
{
    public Dictionary<string, FloatRange> CustomApparelMoney =
        new Dictionary<string, FloatRange>();

    private List<string> customApparelMoneyKeys;

    private List<FloatRange> customApparelMoneyValues;
    private List<string> customApparelTagKeys;
    public Dictionary<string, string> CustomApparelTags;

    private List<string> customApparelTagValues;

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

    public Dictionary<string, float> CustomHeadgearChances =
        new Dictionary<string, float>();

    private List<string> customHeadgearChancesKeys;

    private List<float> customHeadgearChancesValues;

    public Dictionary<string, float> CustomRoyalTitleChances =
        new Dictionary<string, float>();

    private List<string> customRoyalTitleChancesKeys;

    private List<float> customRoyalTitleChancesValues;

    public Dictionary<string, float> CustomTechHediffsChances =
        new Dictionary<string, float>();

    private List<string> customTechHediffsChancesKeys;

    private List<float> customTechHediffsChancesValues;

    public Dictionary<string, FloatRange> CustomTechHediffsMoney =
        new Dictionary<string, FloatRange>();

    private List<string> customTechHediffsMoneyKeys;

    private List<FloatRange> customTechHediffsMoneyValues;

    public Dictionary<string, FloatRange> CustomWeaponMoney =
        new Dictionary<string, FloatRange>();

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
    }

    public void Initialize()
    {
        Biocoding.Initialize();
        ChemicalAddiction.Initialize();
        CombatEnhancingDrugs.Initialize();
        Headgear.Initialize();
        TechHediffs.Initialize();
        TechHediffsMoney.Initialize();
        WeaponMoney.Initialize();
        ApparelMoney.Initialize();
        WeaponTags.Initialize();
        ApparelTags.Initialize();
        if (ModLister.RoyaltyInstalled)
        {
            RoyalTitleChance.Initialize();
        }
    }

    public void ResetValues(string valueLabel)
    {
        if (valueLabel is "biocoding" or "all")
        {
            customBiocodeChancesKeys = new List<string>();
            customBiocodeChancesValues = new List<float>();
            CustomBiocodeChances = new Dictionary<string, float>();
            Biocoding.ResetBiocodingToVanillaRates();
        }

        if (valueLabel is "chemicaladdiction" or "all")
        {
            customChemicalAddictionChancesKeys = new List<string>();
            customChemicalAddictionChancesValues = new List<float>();
            CustomChemicalAddictionChances = new Dictionary<string, float>();
            ChemicalAddiction.ResetChemicalAddictionToVanillaRates();
        }

        if (valueLabel is "combatenhancingdrugs" or "all")
        {
            customCombatEnhancingDrugsChancesKeys = new List<string>();
            customCombatEnhancingDrugsChancesValues = new List<float>();
            CustomCombatEnhancingDrugsChances = new Dictionary<string, float>();
            CombatEnhancingDrugs.ResetCombatEnhancingDrugsToVanillaRates();
        }

        if (valueLabel is "headgear" or "all")
        {
            customHeadgearChancesKeys = new List<string>();
            customHeadgearChancesValues = new List<float>();
            CustomHeadgearChances = new Dictionary<string, float>();
            Headgear.ResetHeadgearToVanillaRates();
        }

        if (valueLabel is "techhediffs" or "all")
        {
            customTechHediffsChancesKeys = new List<string>();
            customTechHediffsChancesValues = new List<float>();
            CustomTechHediffsChances = new Dictionary<string, float>();
            TechHediffs.ResetTechHediffsToVanillaRates();
        }

        if (valueLabel is "techhediffsmoney" or "all")
        {
            customTechHediffsMoneyKeys = new List<string>();
            customTechHediffsMoneyValues = new List<FloatRange>();
            CustomTechHediffsMoney = new Dictionary<string, FloatRange>();
            TechHediffsMoney.ResetTechHediffsMoneyToVanillaRates();
        }

        if (valueLabel is "weaponmoney" or "all")
        {
            customWeaponMoneyKeys = new List<string>();
            customWeaponMoneyValues = new List<FloatRange>();
            CustomWeaponMoney = new Dictionary<string, FloatRange>();
            WeaponMoney.ResetWeaponMoneyToVanillaRates();
        }

        if (valueLabel is "apparelmoney" or "all")
        {
            customApparelMoneyKeys = new List<string>();
            customApparelMoneyValues = new List<FloatRange>();
            CustomApparelMoney = new Dictionary<string, FloatRange>();
            ApparelMoney.ResetApparelMoneyToVanillaRates();
        }

        if (valueLabel is "weapontags" or "all")
        {
            customWeaponTagKeys = new List<string>();
            customWeaponTagValues = new List<string>();
            CustomWeaponTags = new Dictionary<string, string>();
            WeaponTags.ResetWeaponTagsToVanillaValues();
        }

        if (valueLabel is "appareltags" or "all")
        {
            customApparelTagKeys = new List<string>();
            customApparelTagValues = new List<string>();
            CustomApparelTags = new Dictionary<string, string>();
            ApparelTags.ResetApparelTagsToVanillaValues();
        }

        if (valueLabel is "royaltitlechance" or "all")
        {
            customRoyalTitleChancesKeys = new List<string>();
            customRoyalTitleChancesValues = new List<float>();
            CustomRoyalTitleChances = new Dictionary<string, float>();
            RoyalTitleChance.ResetRoyalTitleChanceToVanillaRates();
        }
    }

    public bool HasCustomValues(string type = null)
    {
        if (type is null or "biocoding" && CustomBiocodeChances?.Any() == true)
        {
            return true;
        }

        if (type is null or "chemicaladdiction" && CustomChemicalAddictionChances?.Any() == true)
        {
            return true;
        }

        if (type is null or "combatenhancingdrugs" && CustomCombatEnhancingDrugsChances?.Any() == true)
        {
            return true;
        }

        if (type is null or "headgear" && CustomHeadgearChances?.Any() == true)
        {
            return true;
        }

        if (type is null or "techhediffs" && CustomTechHediffsChances?.Any() == true)
        {
            return true;
        }

        if (type is null or "techhediffsmoney" && CustomTechHediffsMoney?.Any() == true)
        {
            return true;
        }

        if (type is null or "weaponmoney" && CustomWeaponMoney?.Any() == true)
        {
            return true;
        }

        if (type is null or "apparelmoney" && CustomApparelMoney?.Any() == true)
        {
            return true;
        }

        if (type is null or "weapontags" && CustomWeaponTags?.Any() == true)
        {
            return true;
        }

        if (type is null or "appareltags" && CustomApparelTags?.Any() == true)
        {
            return true;
        }

        if (type is null or "royaltitlechance" && CustomRoyalTitleChances?.Any() == true)
        {
            return true;
        }

        return false;
    }
}