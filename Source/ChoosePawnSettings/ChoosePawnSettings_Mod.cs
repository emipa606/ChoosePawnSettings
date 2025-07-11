using System;
using System.Collections.Generic;
using System.Linq;
using Mlie;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ChoosePawnSettings.Settings;

public class ChoosePawnSettings_Mod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static ChoosePawnSettings_Mod instance;

    public static readonly Vector2 buttonSize = new(100f, 25f);

    private static readonly Vector2 searchSize = new(175f, 25f);

    private static readonly int buttonSpacer = 200;

    private static readonly float columnSpacer = 0.1f;

    private static float leftSideWidth;

    private static Listing_Standard listing_Standard;

    private static Vector2 tabsScrollPosition;

    private static string currentVersion;

    private static Vector2 scrollPosition;

    private static string searchText = "";

    public static List<string> CurrentTags;
    public static string TagStage;

    private static readonly Color alternateBackground = new(0.1f, 0.1f, 0.1f, 0.5f);

    private static readonly List<string> settingTabs =
    [
        "Settings",
        null,
        "Biocoding",
        "ChemicalAddiction",
        "CombatEnhancingDrugs",
        "Headgear",
        "CombatPower",
        "TechHediffs",
        "TechHediffsMoney",
        "TechHediffTags",
        "WeaponMoney",
        "WeaponTags",
        "ApparelMoney",
        "ApparelTags",
        "DeathAcidifier",
        "GenerationAge",
        "GenderProbabilities"
    ];

    /// <summary>
    ///     The private settings
    /// </summary>
    public readonly ChoosePawnSettings_Settings Settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public ChoosePawnSettings_Mod(ModContentPack content)
        : base(content)
    {
        instance = this;
        Settings = GetSettings<ChoosePawnSettings_Settings>();
        instance.Settings.CustomBiocodeChances ??= new Dictionary<string, float>();

        if (ModLister.RoyaltyInstalled)
        {
            settingTabs.Add("RoyalTitleChance");
        }

        SelectedDef = "Settings";
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    private string SelectedDef { get; set; }

    public override void WriteSettings()
    {
        base.WriteSettings();
        SelectedDef = "Settings";
    }


    /// <summary>
    ///     The settings-window
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        base.DoSettingsWindowContents(rect);

        var rect2 = rect.ContractedBy(1);
        leftSideWidth = rect2.ContractedBy(10).width / 4;

        listing_Standard = new Listing_Standard();

        drawOptions(rect2);
        drawTabsList(rect2);
        Settings.Write();
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Choose Pawn Settings";
    }


    private static void drawButton(Action action, string text, Vector2 pos)
    {
        var rect = new Rect(pos.x, pos.y, buttonSize.x, buttonSize.y);
        if (!Widgets.ButtonText(rect, text, true, false, Color.white))
        {
            return;
        }

        SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera();
        action();
    }


    private void drawOptions(Rect rect)
    {
        var optionsOuterContainer = rect.ContractedBy(10);
        optionsOuterContainer.x += leftSideWidth + columnSpacer;
        optionsOuterContainer.width -= leftSideWidth + columnSpacer;
        Widgets.DrawBoxSolid(optionsOuterContainer, Color.grey);
        var optionsInnerContainer = optionsOuterContainer.ContractedBy(1);
        Widgets.DrawBoxSolid(optionsInnerContainer, new ColorInt(42, 43, 44).ToColor);
        var frameRect = optionsInnerContainer.ContractedBy(10);
        frameRect.x = leftSideWidth + columnSpacer + 20;
        frameRect.y += 15;
        frameRect.height -= 15;
        var contentRect = frameRect;
        contentRect.x = 0;
        contentRect.y = 0;
        switch (SelectedDef)
        {
            case null:
                return;
            case "Settings":
            {
                listing_Standard.Begin(frameRect);
                Text.Font = GameFont.Medium;
                listing_Standard.Label("CPS.settings".Translate());
                Text.Font = GameFont.Small;
                listing_Standard.Gap();

                if (instance.Settings.HasCustomValues())
                {
                    var labelPoint = listing_Standard.Label("CPS.resetall.label".Translate(), -1F,
                        "CPS.resetall.tooltip".Translate());
                    drawButton(() =>
                        {
                            Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                                "CPS.resetall.confirm".Translate(),
                                delegate { instance.Settings.ResetValues("all"); }));
                        }, "CPS.resetall.button".Translate(),
                        new Vector2(labelPoint.position.x + buttonSpacer, labelPoint.position.y));
                }

                listing_Standard.CheckboxLabeled("CPS.logging.label".Translate(), ref Settings.VerboseLogging,
                    "CPS.logging.tooltip".Translate());
                if (currentVersion != null)
                {
                    listing_Standard.Gap();
                    GUI.contentColor = Color.gray;
                    listing_Standard.Label("CPS.version.label".Translate(currentVersion));
                    GUI.contentColor = Color.white;
                }

                listing_Standard.End();
                break;
            }
            case "Biocoding":
            {
                floatScrollView(ref frameRect, ref instance.Settings.CustomBiocodeChances,
                    Biocoding.VanillaBiocodeChances, "biocoding");
                break;
            }
            case "ChemicalAddiction":
            {
                floatScrollView(ref frameRect, ref instance.Settings.CustomChemicalAddictionChances,
                    ChemicalAddiction.VanillaChemicalAddictionChances, "chemicaladdiction");
                break;
            }
            case "CombatEnhancingDrugs":
            {
                floatScrollView(ref frameRect, ref instance.Settings.CustomCombatEnhancingDrugsChances,
                    CombatEnhancingDrugs.VanillaCombatEnhancingDrugsChances, "combatenhancingdrugs");
                break;
            }
            case "Headgear":
            {
                floatScrollView(ref frameRect, ref instance.Settings.CustomHeadgearChances,
                    Headgear.VanillaHeadgearChances, "headgear");
                break;
            }
            case "CombatPower":
            {
                floatScrollView(ref frameRect, ref instance.Settings.CustomCombatPowers,
                    CombatPower.VanillaCombatPowers, "combatpower");
                break;
            }
            case "TechHediffs":
            {
                floatScrollView(ref frameRect, ref instance.Settings.CustomTechHediffsChances,
                    TechHediffs.VanillaTechHediffsChances, "techhediffs");
                break;
            }
            case "TechHediffsMoney":
            {
                floatRangeScrollView(ref frameRect, ref instance.Settings.CustomTechHediffsMoney,
                    TechHediffsMoney.VanillaTechHediffsMoney, "techhediffsmoney", 8000, 99999);
                break;
            }
            case "WeaponMoney":
            {
                floatRangeScrollView(ref frameRect, ref instance.Settings.CustomWeaponMoney,
                    WeaponMoney.VanillaWeaponMoney, "weaponmoney", 10000, 99999);
                break;
            }
            case "ApparelMoney":
            {
                floatRangeScrollView(ref frameRect, ref instance.Settings.CustomApparelMoney,
                    ApparelMoney.VanillaApparelMoney, "apparelmoney", 12000, 9999999);
                break;
            }
            case "WeaponTags":
            {
                tagsScrollView(ref frameRect, ref instance.Settings.CustomWeaponTags, "weapontags");
                break;
            }
            case "ApparelTags":
            {
                tagsScrollView(ref frameRect, ref instance.Settings.CustomApparelTags, "appareltags");
                break;
            }
            case "TechHediffTags":
            {
                tagsScrollView(ref frameRect, ref instance.Settings.CustomTechHediffTags, "techhedifftags");
                break;
            }
            case "DeathAcidifier":
            {
                boolScrollView(ref frameRect, ref instance.Settings.CustomDeathAcidifier,
                    DeathAcidifier.VanillaDeathAcidifiers, "deathacidifier");
                break;
            }
            case "RoyalTitleChance":
            {
                floatScrollView(ref frameRect, ref instance.Settings.CustomRoyalTitleChances,
                    RoyalTitleChance.VanillaRoyalTitleChances, "royaltitlechance");
                break;
            }
            case "GenderProbabilities":
            {
                genderScrollView(ref frameRect);
                break;
            }
            case "GenerationAge":
            {
                intRangeScrollView(ref frameRect, ref instance.Settings.CustomGenerationAge,
                    GenerationAge.VanillaGenerationAge, "generationage", 1000, 999999);
                break;
            }
        }
    }


    private static void floatScrollView(ref Rect frameRect, ref Dictionary<string, float> modifiedValues,
        Dictionary<string, float> vanillaValues, string header)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label($"CPS.{header}".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), $"CPS.{header}.tooltip".Translate());

        modifiedValues ??= new Dictionary<string, float>();

        if (modifiedValues.Any())
        {
            drawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate($"CPS.{header}".Translate().ToLower()),
                        delegate { instance.Settings.ResetValues(header); }));
                }, "CPS.reset.button".Translate(),
                new Vector2(headerLabel.position.x + headerLabel.width - buttonSize.x,
                    headerLabel.position.y));
        }

        Text.Font = GameFont.Small;

        searchText =
            Widgets.TextField(
                new Rect(headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
                    searchSize),
                searchText);
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
            searchSize), "CPS.search".Translate());

        listing_Standard.End();

        var allPawnKinds = Main.AllPawnKinds;
        if (!string.IsNullOrEmpty(searchText))
        {
            allPawnKinds = Main.AllPawnKinds.Where(def =>
                    def.label?.ToLower().Contains(searchText.ToLower()) == true || def.modContentPack?.Name.ToLower()
                        .Contains(searchText.ToLower()) == true || def.defName.ToLower()
                        .Contains(searchText.ToLower()))
                .ToList();
        }

        var borderRect = frameRect;
        borderRect.y += headerLabel.y + 40;
        borderRect.height -= headerLabel.y + 40;
        var scrollContentRect = frameRect;
        scrollContentRect.height = allPawnKinds.Count * 51f;
        scrollContentRect.width -= 20;
        scrollContentRect.x = 0;
        scrollContentRect.y = 0;

        var scrollListing = new Listing_Standard();
        Widgets.BeginScrollView(borderRect, ref scrollPosition, scrollContentRect);
        scrollListing.Begin(scrollContentRect);
        var alternate = false;
        foreach (var pawnKindDef in allPawnKinds)
        {
            var modInfo = pawnKindDef.modContentPack?.Name;
            var sliderRect = scrollListing.GetRect(50);
            alternate = !alternate;
            if (alternate)
            {
                Widgets.DrawBoxSolid(sliderRect, alternateBackground);
            }

            var pawnKindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnKindLabel.Length > 45)
            {
                pawnKindLabel = $"{pawnKindLabel[..42]}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo[..42]}...";
            }

            switch (header)
            {
                case "biocoding":
                    if (pawnKindDef.biocodeWeaponChance !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.biocodeWeaponChance;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    pawnKindDef.biocodeWeaponChance =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            pawnKindDef.biocodeWeaponChance, 0,
                            1f, false,
                            "CPS.percent".Translate(Math.Round(pawnKindDef.biocodeWeaponChance * 100)),
                            pawnKindLabel,
                            modInfo), 2);
                    break;
                case "chemicaladdiction":
                    if (pawnKindDef.chemicalAddictionChance !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.chemicalAddictionChance;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    pawnKindDef.chemicalAddictionChance =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            pawnKindDef.chemicalAddictionChance, 0,
                            1f, false,
                            "CPS.percent".Translate(Math.Round(pawnKindDef.chemicalAddictionChance * 100)),
                            pawnKindLabel,
                            modInfo), 2);
                    break;
                case "combatenhancingdrugs":
                    if (pawnKindDef.combatEnhancingDrugsChance !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.combatEnhancingDrugsChance;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    pawnKindDef.combatEnhancingDrugsChance =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            pawnKindDef.combatEnhancingDrugsChance, 0,
                            1f, false,
                            "CPS.percent".Translate(Math.Round(pawnKindDef.combatEnhancingDrugsChance * 100)),
                            pawnKindLabel,
                            modInfo), 2);
                    break;
                case "headgear":
                    if (pawnKindDef.apparelAllowHeadgearChance !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.apparelAllowHeadgearChance;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    pawnKindDef.apparelAllowHeadgearChance =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            pawnKindDef.apparelAllowHeadgearChance, 0,
                            1f, false,
                            "CPS.percent".Translate(Math.Round(pawnKindDef.apparelAllowHeadgearChance * 100)),
                            pawnKindLabel,
                            modInfo), 2);
                    break;
                case "combatpower":
                    if (pawnKindDef.combatPower !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.combatPower;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    pawnKindDef.combatPower =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            pawnKindDef.combatPower, 1f,
                            700f, false,
                            Math.Round(pawnKindDef.combatPower).ToString(),
                            pawnKindLabel,
                            modInfo), 2);
                    break;
                case "techhediffs":
                    if (pawnKindDef.techHediffsChance !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.techHediffsChance;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    pawnKindDef.techHediffsChance =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            pawnKindDef.techHediffsChance, 0,
                            1f, false,
                            "CPS.percent".Translate(Math.Round(pawnKindDef.techHediffsChance * 100)),
                            pawnKindLabel,
                            modInfo), 2);
                    break;
                case "royaltitlechance":
                    if (pawnKindDef.royalTitleChance !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.royalTitleChance;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    pawnKindDef.royalTitleChance =
                        (float)Math.Round((decimal)Widgets.HorizontalSlider(
                            sliderRect,
                            pawnKindDef.royalTitleChance, 0,
                            1f, false,
                            "CPS.percent".Translate(Math.Round(pawnKindDef.royalTitleChance * 100)),
                            pawnKindLabel,
                            modInfo), 2);
                    break;
            }

            GUI.color = Color.white;
        }

        scrollListing.End();

        Widgets.EndScrollView();
    }

    private static void genderScrollView(ref Rect frameRect)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label("CPS.genderprobabilities".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), "CPS.genderprobabilities.tooltip".Translate());

        instance.Settings.CustomGenderProbabilities ??= new Dictionary<string, float>();

        if (instance.Settings.CustomGenderProbabilities.Any())
        {
            drawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate("CPS.genderprobabilities".Translate().ToLower()),
                        delegate { instance.Settings.ResetValues("genderprobabilities"); }));
                }, "CPS.reset.button".Translate(),
                new Vector2(headerLabel.position.x + headerLabel.width - buttonSize.x,
                    headerLabel.position.y));
        }

        Text.Font = GameFont.Small;

        searchText =
            Widgets.TextField(
                new Rect(headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
                    searchSize),
                searchText);
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
            searchSize), "CPS.search".Translate());

        listing_Standard.End();

        var allPawnKinds = Main.AllPawnKinds;
        if (!string.IsNullOrEmpty(searchText))
        {
            allPawnKinds = Main.AllPawnKinds.Where(def =>
                    def.label?.ToLower().Contains(searchText.ToLower()) == true || def.modContentPack?.Name.ToLower()
                        .Contains(searchText.ToLower()) == true || def.defName.ToLower()
                        .Contains(searchText.ToLower()))
                .ToList();
        }

        var borderRect = frameRect;
        borderRect.y += headerLabel.y + 40;
        borderRect.height -= headerLabel.y + 40;
        var scrollContentRect = frameRect;
        scrollContentRect.height = allPawnKinds.Count * 66f;
        scrollContentRect.width -= 20;
        scrollContentRect.x = 0;
        scrollContentRect.y = 0;

        var scrollListing = new Listing_Standard();
        Widgets.BeginScrollView(borderRect, ref scrollPosition, scrollContentRect);
        scrollListing.Begin(scrollContentRect);
        var alternate = false;
        foreach (var pawnKindDef in allPawnKinds)
        {
            var modInfo = pawnKindDef.modContentPack?.Name;
            var sliderRect = scrollListing.GetRect(65);
            var middleRect = sliderRect.ContractedBy(5, 5f);
            middleRect.y -= 5f;
            alternate = !alternate;
            if (alternate)
            {
                Widgets.DrawBoxSolid(sliderRect, alternateBackground);
            }

            var pawnkindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnkindLabel.Length > 45)
            {
                pawnkindLabel = $"{pawnkindLabel[..42]}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo[..42]}...";
            }

            pawnkindLabel = "Male".Translate().CapitalizeFirst() + "\n\n" + pawnkindLabel;
            modInfo = "Female".Translate().CapitalizeFirst() + "\n\n" + modInfo;

            var currentChance = 0.5f;

            if (instance.Settings.CustomGenderProbabilities.ContainsKey(pawnKindDef.defName))
            {
                if (Math.Round(instance.Settings.CustomGenderProbabilities[pawnKindDef.defName], 2) == 0.5f)
                {
                    instance.Settings.CustomGenderProbabilities.Remove(pawnKindDef.defName);
                }
                else
                {
                    currentChance = instance.Settings.CustomGenderProbabilities[pawnKindDef.defName];
                    GUI.color = Color.green;
                }
            }

            currentChance =
                (float)Math.Round((decimal)Widgets.HorizontalSlider(
                    middleRect,
                    currentChance, 0,
                    1f, false,
                    $"{(1f - currentChance).ToStringPercent()}/{currentChance.ToStringPercent()}",
                    pawnkindLabel,
                    modInfo), 2);

            if (currentChance != 0.5f)
            {
                instance.Settings.CustomGenderProbabilities[pawnKindDef.defName] = currentChance;
            }
            else
            {
                instance.Settings.CustomGenderProbabilities.Remove(pawnKindDef.defName);
            }

            GUI.color = Color.white;
        }

        scrollListing.End();

        Widgets.EndScrollView();
    }

    private static void floatRangeScrollView(ref Rect frameRect, ref Dictionary<string, FloatRange> modifiedValues,
        Dictionary<string, FloatRange> vanillaValues, string header, int maxValue, int unlimitedValue)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label($"CPS.{header}".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), $"CPS.{header}.tooltip".Translate());

        modifiedValues ??= new Dictionary<string, FloatRange>();

        if (modifiedValues.Any())
        {
            drawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate($"CPS.{header}".Translate().ToLower()),
                        delegate { instance.Settings.ResetValues(header); }));
                }, "CPS.reset.button".Translate(),
                new Vector2(headerLabel.position.x + headerLabel.width - buttonSize.x,
                    headerLabel.position.y));
        }

        Text.Font = GameFont.Small;

        searchText =
            Widgets.TextField(
                new Rect(headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
                    searchSize),
                searchText);
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
            searchSize), "CPS.search".Translate());

        listing_Standard.End();

        var allPawnKinds = Main.AllPawnKinds;
        if (!string.IsNullOrEmpty(searchText))
        {
            allPawnKinds = Main.AllPawnKinds.Where(def =>
                    def.label?.ToLower().Contains(searchText.ToLower()) == true || def.modContentPack?.Name.ToLower()
                        .Contains(searchText.ToLower()) == true || def.defName.ToLower()
                        .Contains(searchText.ToLower()))
                .ToList();
        }

        var borderRect = frameRect;
        borderRect.y += headerLabel.y + 40;
        borderRect.height -= headerLabel.y + 40;
        var scrollContentRect = frameRect;
        scrollContentRect.height = allPawnKinds.Count * 81f;
        scrollContentRect.width -= 20;
        scrollContentRect.x = 0;
        scrollContentRect.y = 0;

        var scrollListing = new Listing_Standard();
        Widgets.BeginScrollView(borderRect, ref scrollPosition, scrollContentRect);
        scrollListing.Begin(scrollContentRect);
        var alternate = false;
        foreach (var pawnKindDef in allPawnKinds)
        {
            alternate = !alternate;
            var sliderRect = scrollListing.GetRect(70);
            if (alternate)
            {
                Widgets.DrawBoxSolid(sliderRect, alternateBackground);
            }

            Text.Font = GameFont.Tiny;
            var modInfo = pawnKindDef.modContentPack?.Name;
            var pawnKindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnKindLabel.Length > 45)
            {
                pawnKindLabel = $"{pawnKindLabel[..42]}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo[..42]}...";
            }

            var smallerRect = sliderRect.ContractedBy(4f, 20f);
            smallerRect.width -= 100f;
            smallerRect.y += 5;
            smallerRect.x += 2;
            var checkboxRect = sliderRect;
            checkboxRect.width = sliderRect.width - smallerRect.width - 10f;
            checkboxRect.height -= 20f;
            checkboxRect.y += 20f;
            checkboxRect.x = sliderRect.x + smallerRect.width + 10f;
            bool unlimited;
            bool wasOn;
            switch (header)
            {
                case "techhediffsmoney":
                    if (pawnKindDef.techHediffsMoney !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.techHediffsMoney;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    unlimited = pawnKindDef.techHediffsMoney.max > maxValue;
                    wasOn = unlimited;
                    Widgets.CheckboxLabeled(checkboxRect, "CPS.unlimited".Translate(), ref unlimited);
                    Widgets.FloatRange(
                        smallerRect,
                        pawnKindDef.GetHashCode(),
                        ref pawnKindDef.techHediffsMoney,
                        0,
                        maxValue,
                        null,
                        ToStringStyle.Money);
                    if (unlimited)
                    {
                        pawnKindDef.techHediffsMoney.max = unlimitedValue;
                        pawnKindDef.techHediffsMoney.min = unlimitedValue;
                    }
                    else
                    {
                        if (wasOn)
                        {
                            pawnKindDef.techHediffsMoney.max = maxValue / 2f;
                            pawnKindDef.techHediffsMoney.min = maxValue / 2f;
                        }
                    }

                    break;
                case "weaponmoney":
                    if (pawnKindDef.weaponMoney !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.weaponMoney;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    unlimited = pawnKindDef.weaponMoney.max > maxValue;
                    wasOn = unlimited;
                    Widgets.CheckboxLabeled(checkboxRect, "CPS.unlimited".Translate(), ref unlimited);
                    Widgets.FloatRange(
                        smallerRect,
                        pawnKindDef.GetHashCode(),
                        ref pawnKindDef.weaponMoney,
                        0,
                        maxValue,
                        null,
                        ToStringStyle.Money);
                    if (unlimited)
                    {
                        pawnKindDef.weaponMoney.max = unlimitedValue;
                        pawnKindDef.weaponMoney.min = unlimitedValue;
                    }
                    else
                    {
                        if (wasOn)
                        {
                            pawnKindDef.weaponMoney.max = maxValue / 2f;
                            pawnKindDef.weaponMoney.min = maxValue / 2f;
                        }
                    }

                    break;
                case "apparelmoney":
                    if (pawnKindDef.apparelMoney !=
                        vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            pawnKindDef.apparelMoney;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    unlimited = pawnKindDef.apparelMoney.max > maxValue;
                    wasOn = unlimited;
                    Widgets.CheckboxLabeled(checkboxRect, "CPS.unlimited".Translate(), ref unlimited);
                    Widgets.FloatRange(
                        smallerRect,
                        pawnKindDef.GetHashCode(),
                        ref pawnKindDef.apparelMoney,
                        0,
                        maxValue,
                        null,
                        ToStringStyle.Money);
                    if (unlimited)
                    {
                        pawnKindDef.apparelMoney.max = unlimitedValue;
                        pawnKindDef.apparelMoney.min = unlimitedValue;
                    }
                    else
                    {
                        if (wasOn)
                        {
                            pawnKindDef.apparelMoney.max = maxValue / 2f;
                            pawnKindDef.apparelMoney.min = maxValue / 2f;
                        }
                    }

                    break;
            }

            var textRect = sliderRect;
            textRect.width -= 100f;
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.Label(textRect, pawnKindLabel);
            Text.Anchor = TextAnchor.UpperRight;
            Widgets.Label(textRect, modInfo);
            Text.Anchor = default;
            GUI.color = Color.white;
            scrollListing.Gap(10);
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    private static void intRangeScrollView(ref Rect frameRect, ref Dictionary<string, IntRange> modifiedValues,
        Dictionary<string, IntRange> vanillaValues, string header, int maxValue, int unlimitedValue)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label($"CPS.{header}".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), $"CPS.{header}.tooltip".Translate());

        modifiedValues ??= new Dictionary<string, IntRange>();

        if (modifiedValues.Any())
        {
            drawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate($"CPS.{header}".Translate().ToLower()),
                        delegate { instance.Settings.ResetValues(header); }));
                }, "CPS.reset.button".Translate(),
                new Vector2(headerLabel.position.x + headerLabel.width - buttonSize.x,
                    headerLabel.position.y));
        }

        Text.Font = GameFont.Small;

        searchText =
            Widgets.TextField(
                new Rect(headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
                    searchSize),
                searchText);
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
            searchSize), "CPS.search".Translate());

        listing_Standard.End();

        var allPawnKinds = Main.AllPawnKinds;
        if (!string.IsNullOrEmpty(searchText))
        {
            allPawnKinds = Main.AllPawnKinds.Where(def =>
                    def.label?.ToLower().Contains(searchText.ToLower()) == true || def.modContentPack?.Name.ToLower()
                        .Contains(searchText.ToLower()) == true || def.defName.ToLower()
                        .Contains(searchText.ToLower()))
                .ToList();
        }

        var borderRect = frameRect;
        borderRect.y += headerLabel.y + 40;
        borderRect.height -= headerLabel.y + 40;
        var scrollContentRect = frameRect;
        scrollContentRect.height = allPawnKinds.Count * 81f;
        scrollContentRect.width -= 20;
        scrollContentRect.x = 0;
        scrollContentRect.y = 0;

        var scrollListing = new Listing_Standard();
        Widgets.BeginScrollView(borderRect, ref scrollPosition, scrollContentRect);
        scrollListing.Begin(scrollContentRect);
        var alternate = false;
        foreach (var pawnKindDef in allPawnKinds)
        {
            alternate = !alternate;
            var sliderRect = scrollListing.GetRect(70);
            if (alternate)
            {
                Widgets.DrawBoxSolid(sliderRect, alternateBackground);
            }

            Text.Font = GameFont.Tiny;
            var modInfo = pawnKindDef.modContentPack?.Name;
            var pawnKindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnKindLabel.Length > 45)
            {
                pawnKindLabel = $"{pawnKindLabel[..42]}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo[..42]}...";
            }

            var smallerRect = sliderRect.ContractedBy(4f, 20f);
            smallerRect.width -= 100f;
            smallerRect.y += 5;
            smallerRect.x += 2;
            var checkboxRect = sliderRect;
            checkboxRect.width = sliderRect.width - smallerRect.width - 10f;
            checkboxRect.height -= 20f;
            checkboxRect.y += 20f;
            checkboxRect.x = sliderRect.x + smallerRect.width + 10f;
            switch (header)
            {
                case "generationage":
                    if (pawnKindDef.minGenerationAge != vanillaValues[pawnKindDef.defName].min ||
                        pawnKindDef.maxGenerationAge != vanillaValues[pawnKindDef.defName].max)
                    {
                        modifiedValues[pawnKindDef.defName] =
                            new IntRange(pawnKindDef.minGenerationAge, pawnKindDef.maxGenerationAge);
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    var unlimited = pawnKindDef.maxGenerationAge > maxValue;
                    var wasOn = unlimited;
                    Widgets.CheckboxLabeled(checkboxRect, "CPS.unlimited".Translate(), ref unlimited);
                    var tempRange = new IntRange(pawnKindDef.minGenerationAge, pawnKindDef.maxGenerationAge);
                    Widgets.IntRange(
                        smallerRect,
                        pawnKindDef.GetHashCode(),
                        ref tempRange,
                        0,
                        maxValue);
                    if (wasOn != unlimited || pawnKindDef.minGenerationAge != tempRange.min ||
                        pawnKindDef.maxGenerationAge != tempRange.max)
                    {
                        pawnKindDef.minGenerationAge = tempRange.min;
                        if (unlimited)
                        {
                            pawnKindDef.maxGenerationAge = unlimitedValue;
                        }
                        else
                        {
                            if (wasOn)
                            {
                                pawnKindDef.maxGenerationAge = maxValue / 2;
                            }
                            else
                            {
                                pawnKindDef.maxGenerationAge = tempRange.max;
                            }
                        }
                    }

                    break;
            }

            var textRect = sliderRect;
            textRect.width -= 100f;
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.Label(textRect, pawnKindLabel);
            Text.Anchor = TextAnchor.UpperRight;
            Widgets.Label(textRect, modInfo);
            Text.Anchor = default;
            GUI.color = Color.white;
            scrollListing.Gap(10);
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    private static void tagsScrollView(ref Rect frameRect, ref Dictionary<string, string> modifiedValues, string header)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label($"CPS.{header}".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), $"CPS.{header}.tooltip".Translate());

        modifiedValues ??= new Dictionary<string, string>();

        if (modifiedValues.Any())
        {
            drawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate($"CPS.{header}".Translate().ToLower()),
                        delegate { instance.Settings.ResetValues(header); }));
                }, "CPS.reset.button".Translate(),
                new Vector2(headerLabel.position.x + headerLabel.width - buttonSize.x,
                    headerLabel.position.y));
        }

        Text.Font = GameFont.Small;

        searchText =
            Widgets.TextField(
                new Rect(headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
                    searchSize),
                searchText);
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
            searchSize), "CPS.search".Translate());

        listing_Standard.End();

        var allPawnKinds = Main.AllPawnKinds;
        if (!string.IsNullOrEmpty(searchText))
        {
            allPawnKinds = Main.AllPawnKinds.Where(def =>
                    def.label?.ToLower().Contains(searchText.ToLower()) == true || def.modContentPack?.Name.ToLower()
                        .Contains(searchText.ToLower()) == true || def.defName.ToLower()
                        .Contains(searchText.ToLower()))
                .ToList();
        }

        var borderRect = frameRect;
        borderRect.y += headerLabel.y + 40;
        borderRect.height -= headerLabel.y + 40;
        var scrollContentRect = frameRect;
        scrollContentRect.height = allPawnKinds.Count * 61f;
        scrollContentRect.width -= 20;
        scrollContentRect.x = 0;
        scrollContentRect.y = 0;

        var scrollListing = new Listing_Standard();
        Widgets.BeginScrollView(borderRect, ref scrollPosition, scrollContentRect);
        scrollListing.Begin(scrollContentRect);
        var alternate = false;
        foreach (var pawnKindDef in allPawnKinds)
        {
            alternate = !alternate;
            var sliderRect = scrollListing.GetRect(50);
            if (alternate)
            {
                Widgets.DrawBoxSolid(sliderRect, alternateBackground);
            }

            var modInfo = pawnKindDef.modContentPack?.Name;
            var pawnKindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnKindLabel.Length > 45)
            {
                pawnKindLabel = $"{pawnKindLabel[..42]}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo[..42]}...";
            }

            var smallerRect = sliderRect.ContractedBy(4f);
            smallerRect.width -= buttonSize.x + 2;
            smallerRect.y += 15;
            smallerRect.x += buttonSize.x + 2;
            var buttonRect = smallerRect;
            buttonRect.size = buttonSize;
            buttonRect.x -= buttonSize.x + 2;
            int tagCount;
            string tags;
            switch (header)
            {
                case "weapontags":
                    if (TagStage == pawnKindDef.defName)
                    {
                        if (CurrentTags == null ||
                            CurrentTags == WeaponTags.VanillaWeaponTagsDictionary[pawnKindDef.defName])
                        {
                            WeaponTags.ResetWeaponTagsToVanillaValues(pawnKindDef.defName);
                        }
                        else
                        {
                            pawnKindDef.weaponTags = CurrentTags;
                            modifiedValues[pawnKindDef.defName] = string.Join("|", pawnKindDef.weaponTags);
                        }

                        TagStage = null;
                    }

                    tagCount = 0;
                    tags = string.Empty;
                    if (pawnKindDef.weaponTags != null)
                    {
                        tagCount = pawnKindDef.weaponTags.Count;
                        tags = string.Join("\n", pawnKindDef.weaponTags.OrderBy(s => s));
                    }

                    if (Widgets.ButtonText(buttonRect, "CPS.edit".Translate()))
                    {
                        CurrentTags = pawnKindDef.weaponTags;
                        TagStage = "selecting";
                        Find.WindowStack.Add(new Dialog_ChooseTags(WeaponTags.WeaponTagDictionary,
                            pawnKindDef.defName));
                    }

                    if (modifiedValues.ContainsKey(pawnKindDef.defName))
                    {
                        GUI.color = Color.green;
                    }

                    Widgets.Label(smallerRect, "CPS.currenttags".Translate(tagCount));
                    TooltipHandler.TipRegion(smallerRect, tags);

                    break;
                case "appareltags":
                    if (TagStage == pawnKindDef.defName)
                    {
                        if (CurrentTags == null ||
                            CurrentTags == ApparelTags.VanillaApparelTagsDictionary[pawnKindDef.defName])
                        {
                            ApparelTags.ResetApparelTagsToVanillaValues(pawnKindDef.defName);
                        }
                        else
                        {
                            pawnKindDef.apparelTags = CurrentTags;
                            modifiedValues[pawnKindDef.defName] = string.Join("|", pawnKindDef.apparelTags);
                        }

                        TagStage = null;
                    }


                    tagCount = 0;
                    tags = string.Empty;
                    if (pawnKindDef.apparelTags != null)
                    {
                        tagCount = pawnKindDef.apparelTags.Count;
                        tags = string.Join("\n", pawnKindDef.apparelTags.OrderBy(s => s));
                    }

                    if (Widgets.ButtonText(buttonRect, "CPS.edit".Translate()))
                    {
                        CurrentTags = pawnKindDef.apparelTags;
                        TagStage = "selecting";
                        Find.WindowStack.Add(new Dialog_ChooseTags(ApparelTags.ApparelTagDictionary,
                            pawnKindDef.defName));
                    }

                    if (modifiedValues.ContainsKey(pawnKindDef.defName))
                    {
                        GUI.color = Color.green;
                    }

                    Widgets.Label(smallerRect, "CPS.currenttags".Translate(tagCount));
                    TooltipHandler.TipRegion(smallerRect, tags);

                    break;
                case "techhedifftags":
                    if (TagStage == pawnKindDef.defName)
                    {
                        if (CurrentTags == null ||
                            CurrentTags == TechHediffTags.VanillaTechHediffTagDictionary[pawnKindDef.defName])
                        {
                            TechHediffTags.ResetTechHediffTagsToVanillaValues(pawnKindDef.defName);
                        }
                        else
                        {
                            pawnKindDef.techHediffsTags = CurrentTags;
                            modifiedValues[pawnKindDef.defName] = string.Join("|", pawnKindDef.techHediffsTags);
                        }

                        TagStage = null;
                    }


                    tagCount = 0;
                    tags = string.Empty;
                    if (pawnKindDef.techHediffsTags != null)
                    {
                        tagCount = pawnKindDef.techHediffsTags.Count;
                        tags = string.Join("\n", pawnKindDef.techHediffsTags.OrderBy(s => s));
                    }

                    if (Widgets.ButtonText(buttonRect, "CPS.edit".Translate()))
                    {
                        CurrentTags = pawnKindDef.techHediffsTags;
                        TagStage = "selecting";
                        Find.WindowStack.Add(new Dialog_ChooseTags(TechHediffTags.TechHediffTagDictionary,
                            pawnKindDef.defName));
                    }

                    if (modifiedValues.ContainsKey(pawnKindDef.defName))
                    {
                        GUI.color = Color.green;
                    }

                    Widgets.Label(smallerRect, "CPS.currenttags".Translate(tagCount));
                    TooltipHandler.TipRegion(smallerRect, tags);

                    break;
            }

            GUI.color = Color.white;
            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.Label(sliderRect, pawnKindLabel);
            Text.Anchor = TextAnchor.UpperRight;
            Widgets.Label(sliderRect, modInfo);
            Text.Anchor = default;
            GUI.color = Color.white;
            scrollListing.Gap(10);
            Text.Font = GameFont.Small;
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    private static void boolScrollView(ref Rect frameRect, ref Dictionary<string, bool> modifiedValues,
        Dictionary<string, bool> vanillaValues, string header)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label($"CPS.{header}".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), $"CPS.{header}.tooltip".Translate());

        modifiedValues ??= new Dictionary<string, bool>();

        if (modifiedValues.Any())
        {
            drawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate($"CPS.{header}".Translate().ToLower()),
                        delegate { instance.Settings.ResetValues(header); }));
                }, "CPS.reset.button".Translate(),
                new Vector2(headerLabel.position.x + headerLabel.width - buttonSize.x,
                    headerLabel.position.y));
        }

        Text.Font = GameFont.Small;

        searchText =
            Widgets.TextField(
                new Rect(headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
                    searchSize),
                searchText);
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position + new Vector2((frameRect.width / 3 * 2) - (searchSize.x / 2), 0),
            searchSize), "CPS.search".Translate());

        listing_Standard.End();

        var allPawnKinds = Main.AllPawnKinds;
        if (!string.IsNullOrEmpty(searchText))
        {
            allPawnKinds = Main.AllPawnKinds.Where(def =>
                    def.label?.ToLower().Contains(searchText.ToLower()) == true || def.modContentPack?.Name.ToLower()
                        .Contains(searchText.ToLower()) == true || def.defName.ToLower()
                        .Contains(searchText.ToLower()))
                .ToList();
        }

        var borderRect = frameRect;
        borderRect.y += headerLabel.y + 40;
        borderRect.height -= headerLabel.y + 40;
        var scrollContentRect = frameRect;
        scrollContentRect.height = allPawnKinds.Count * 51f;
        scrollContentRect.width -= 20;
        scrollContentRect.x = 0;
        scrollContentRect.y = 0;

        var scrollListing = new Listing_Standard();
        Widgets.BeginScrollView(borderRect, ref scrollPosition, scrollContentRect);
        scrollListing.Begin(scrollContentRect);
        var alternate = false;
        foreach (var pawnKindDef in allPawnKinds)
        {
            var modInfo = pawnKindDef.modContentPack?.Name;
            var sliderRect = scrollListing.GetRect(50);
            alternate = !alternate;
            if (alternate)
            {
                Widgets.DrawBoxSolid(sliderRect, alternateBackground);
            }

            var pawnKindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnKindLabel.Length > 65)
            {
                pawnKindLabel = $"{pawnKindLabel[..62]}...";
            }

            if (modInfo is { Length: > 65 })
            {
                modInfo = $"{modInfo[..62]}...";
            }

            switch (header)
            {
                case "deathacidifier":
                    var hasDeathAcidifier =
                        pawnKindDef.techHediffsRequired?.Contains(DeathAcidifier.DeathAcidifierThingDef) == true;
                    if (hasDeathAcidifier != vanillaValues[pawnKindDef.defName])
                    {
                        modifiedValues[pawnKindDef.defName] =
                            hasDeathAcidifier;
                        GUI.color = Color.green;
                    }
                    else
                    {
                        modifiedValues.Remove(pawnKindDef.defName);
                    }

                    Widgets.CheckboxLabeled(sliderRect, $"{pawnKindLabel} - {modInfo}", ref hasDeathAcidifier);

                    if (hasDeathAcidifier)
                    {
                        pawnKindDef.techHediffsRequired ??= [];

                        if (!pawnKindDef.techHediffsRequired.Contains(DeathAcidifier.DeathAcidifierThingDef))
                        {
                            pawnKindDef.techHediffsRequired.Add(DeathAcidifier.DeathAcidifierThingDef);
                        }
                    }
                    else
                    {
                        if (pawnKindDef.techHediffsRequired?.Contains(DeathAcidifier.DeathAcidifierThingDef) == true)
                        {
                            pawnKindDef.techHediffsRequired.Remove(DeathAcidifier.DeathAcidifierThingDef);
                        }
                    }

                    break;
            }

            GUI.color = Color.white;
        }

        scrollListing.End();

        Widgets.EndScrollView();
    }

    private void drawTabsList(Rect rect)
    {
        var scrollContainer = rect.ContractedBy(10);
        scrollContainer.width = leftSideWidth;
        Widgets.DrawBoxSolid(scrollContainer, Color.grey);
        var innerContainer = scrollContainer.ContractedBy(1);
        Widgets.DrawBoxSolid(innerContainer, new ColorInt(42, 43, 44).ToColor);
        var tabFrameRect = innerContainer.ContractedBy(5);
        tabFrameRect.y += 15;
        tabFrameRect.height -= 15;
        var tabContentRect = tabFrameRect;
        tabContentRect.x = 0;
        tabContentRect.y = 0;
        tabContentRect.width -= 20;

        const int listAddition = 24;


        tabContentRect.height = (settingTabs.Count * 25f) + listAddition;
        Widgets.BeginScrollView(tabFrameRect, ref tabsScrollPosition, tabContentRect);
        listing_Standard.Begin(tabContentRect);
        //Text.Font = GameFont.Tiny;
        foreach (var settingTab in settingTabs)
        {
            if (string.IsNullOrEmpty(settingTab))
            {
                listing_Standard.ListItemSelectable(null, Color.yellow);
                continue;
            }

            if (instance.Settings.HasCustomValues(settingTab.ToLower()))
            {
                GUI.color = Color.green;
            }

            if (listing_Standard.ListItemSelectable($"CPS.{settingTab.ToLower()}".Translate(), Color.yellow,
                    SelectedDef == settingTab))
            {
                SelectedDef = SelectedDef == settingTab ? null : settingTab;
            }

            GUI.color = Color.white;
        }

        listing_Standard.End();
        //Text.Font = GameFont.Small;
        Widgets.EndScrollView();
    }
}