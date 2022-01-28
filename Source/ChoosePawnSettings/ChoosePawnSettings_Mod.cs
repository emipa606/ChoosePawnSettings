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

    private static readonly Vector2 buttonSize = new Vector2(100f, 25f);

    private static readonly Vector2 searchSize = new Vector2(175f, 25f);

    private static readonly int buttonSpacer = 200;

    private static readonly float columnSpacer = 0.1f;

    private static float leftSideWidth;

    private static Listing_Standard listing_Standard;

    private static Vector2 tabsScrollPosition;

    private static string currentVersion;

    private static Vector2 scrollPosition;

    private static string searchText = "";

    private static readonly Color alternateBackground = new Color(0.1f, 0.1f, 0.1f, 0.5f);

    private static readonly List<string> settingTabs = new List<string>
    {
        "Settings",
        null,
        "Biocoding",
        "ChemicalAddiction",
        "CombatEnhancingDrugs",
        "Headgear",
        "TechHediffs",
        "TechHediffsMoney"
    };

    /// <summary>
    ///     The private settings
    /// </summary>
    private ChoosePawnSettings_Settings settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public ChoosePawnSettings_Mod(ModContentPack content)
        : base(content)
    {
        instance = this;
        if (instance.Settings.CustomBiocodeChances == null)
        {
            instance.Settings.CustomBiocodeChances = new Dictionary<string, float>();
        }

        if (ModLister.RoyaltyInstalled)
        {
            settingTabs.Add("RoyalTitleChance");
        }

        SelectedDef = "Settings";
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.ChoosePawnSettings"));
    }

    /// <summary>
    ///     The instance-settings for the mod
    /// </summary>
    internal ChoosePawnSettings_Settings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = GetSettings<ChoosePawnSettings_Settings>();
            }

            return settings;
        }

        set => settings = value;
    }

    public string SelectedDef { get; set; }

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

        DrawOptions(rect2);
        DrawTabsList(rect2);
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


    private static void DrawButton(Action action, string text, Vector2 pos)
    {
        var rect = new Rect(pos.x, pos.y, buttonSize.x, buttonSize.y);
        if (!Widgets.ButtonText(rect, text, true, false, Color.white))
        {
            return;
        }

        SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera();
        action();
    }


    private void DrawIcon(PawnKindDef pawnKind, Rect rect)
    {
        Main.LogMessage($"Draw icon for {pawnKind}");
        //var texture2D = pawnKind?.lifeStages?.Last()?.bodyGraphicData?.Graphic?.MatSingle?.mainTexture;
        var texture2D = pawnKind?.race?.graphicData?.Graphic?.MatSingle?.mainTexture;

        if (texture2D == null)
        {
            return;
        }

        var toolTip = $"{pawnKind.LabelCap}\n{pawnKind.race?.description}";
        if (texture2D.width != texture2D.height)
        {
            var ratio = (float)texture2D.width / texture2D.height;

            if (ratio < 1)
            {
                rect.x += (rect.width - (rect.width * ratio)) / 2;
                rect.width *= ratio;
            }
            else
            {
                rect.y += (rect.height - (rect.height / ratio)) / 2;
                rect.height /= ratio;
            }
        }

        GUI.DrawTexture(rect, texture2D);
        TooltipHandler.TipRegion(rect, toolTip);
    }

    private void DrawOptions(Rect rect)
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

                if (instance.Settings.CustomBiocodeChances?.Any() == true ||
                    instance.Settings.CustomChemicalAddictionChances?.Any() == true ||
                    instance.Settings.CustomCombatEnhancingDrugsChances?.Any() == true ||
                    instance.Settings.CustomHeadgearChances?.Any() == true)
                {
                    var labelPoint = listing_Standard.Label("CPS.resetall.label".Translate(), -1F,
                        "CPS.resetall.tooltip".Translate());
                    DrawButton(() =>
                        {
                            Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                                "CPS.resetall.confirm".Translate(),
                                delegate { instance.Settings.ResetManualValues(); }));
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
                biocodingScrollView(ref frameRect);
                break;
            }
            case "ChemicalAddiction":
            {
                chemicaladdictionScrollView(ref frameRect);
                break;
            }
            case "CombatEnhancingDrugs":
            {
                combatenhancingdrugsScrollView(ref frameRect);
                break;
            }
            case "Headgear":
            {
                headgearScrollView(ref frameRect);
                break;
            }
            case "TechHediffs":
            {
                techhediffsScrollView(ref frameRect);
                break;
            }
            case "TechHediffsMoney":
            {
                techhediffsmoneyScrollView(ref frameRect);
                break;
            }
            case "RoyalTitleChance":
            {
                royalTitleChanceScrollView(ref frameRect);
                break;
            }
        }
    }

    private void biocodingScrollView(ref Rect frameRect)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label("CPS.biocoding".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), "CPS.biocoding.tooltip".Translate());

        if (instance.Settings.CustomBiocodeChances == null)
        {
            instance.Settings.CustomBiocodeChances = new Dictionary<string, float>();
        }

        if (instance.Settings.CustomBiocodeChances.Any())
        {
            DrawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate("CPS.biocoding".Translate().ToLower()),
                        delegate { instance.Settings.ResetBiocodeValues(); }));
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
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack.Name.ToLower()
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

            var pawnkindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnkindLabel.Length > 45)
            {
                pawnkindLabel = $"{pawnkindLabel.Substring(0, 42)}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo.Substring(0, 42)}...";
            }

            if (pawnKindDef.biocodeWeaponChance != Biocoding.VanillaBiocodeChances[pawnKindDef.defName])
            {
                instance.Settings.CustomBiocodeChances[pawnKindDef.defName] =
                    pawnKindDef.biocodeWeaponChance;
                GUI.color = Color.green;
            }
            else
            {
                if (instance.Settings.CustomBiocodeChances.ContainsKey(pawnKindDef.defName))
                {
                    instance.Settings.CustomBiocodeChances.Remove(pawnKindDef.defName);
                }
            }

            pawnKindDef.biocodeWeaponChance =
                (float)Math.Round((decimal)Widgets.HorizontalSlider(
                    sliderRect,
                    pawnKindDef.biocodeWeaponChance, 0,
                    1f, false,
                    "CPS.percent".Translate(Math.Round(pawnKindDef.biocodeWeaponChance * 100)),
                    pawnkindLabel,
                    modInfo), 2);

            GUI.color = Color.white;
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    private void chemicaladdictionScrollView(ref Rect frameRect)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label("CPS.chemicaladdiction".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), "CPS.chemicaladdiction.tooltip".Translate());

        if (instance.Settings.CustomChemicalAddictionChances == null)
        {
            instance.Settings.CustomChemicalAddictionChances = new Dictionary<string, float>();
        }

        if (instance.Settings.CustomChemicalAddictionChances.Any())
        {
            DrawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate("CPS.chemicaladdiction".Translate().ToLower()),
                        delegate { instance.Settings.ResetChemicalAddictionValues(); }));
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
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack.Name.ToLower()
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

            var pawnkindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnkindLabel.Length > 45)
            {
                pawnkindLabel = $"{pawnkindLabel.Substring(0, 42)}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo.Substring(0, 42)}...";
            }

            if (pawnKindDef.chemicalAddictionChance !=
                ChemicalAddiction.VanillaChemicalAddictionChances[pawnKindDef.defName])
            {
                instance.Settings.CustomChemicalAddictionChances[pawnKindDef.defName] =
                    pawnKindDef.chemicalAddictionChance;
                GUI.color = Color.green;
            }
            else
            {
                if (instance.Settings.CustomChemicalAddictionChances.ContainsKey(pawnKindDef.defName))
                {
                    instance.Settings.CustomChemicalAddictionChances.Remove(pawnKindDef.defName);
                }
            }

            pawnKindDef.chemicalAddictionChance =
                (float)Math.Round((decimal)Widgets.HorizontalSlider(
                    sliderRect,
                    pawnKindDef.chemicalAddictionChance, 0,
                    1f, false,
                    "CPS.percent".Translate(Math.Round(pawnKindDef.chemicalAddictionChance * 100)),
                    pawnkindLabel,
                    modInfo), 2);

            GUI.color = Color.white;
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    private void combatenhancingdrugsScrollView(ref Rect frameRect)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label("CPS.combatenhancingdrugs".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), "CPS.combatenhancingdrugs.tooltip".Translate());

        if (instance.Settings.CustomCombatEnhancingDrugsChances == null)
        {
            instance.Settings.CustomCombatEnhancingDrugsChances = new Dictionary<string, float>();
        }

        if (instance.Settings.CustomCombatEnhancingDrugsChances.Any())
        {
            DrawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate("CPS.combatenhancingdrugs".Translate().ToLower()),
                        delegate { instance.Settings.ResetCombatEnhancingDrugsValues(); }));
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
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack.Name.ToLower()
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

            var pawnkindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnkindLabel.Length > 45)
            {
                pawnkindLabel = $"{pawnkindLabel.Substring(0, 42)}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo.Substring(0, 42)}...";
            }

            if (pawnKindDef.combatEnhancingDrugsChance !=
                CombatEnhancingDrugs.VanillaCombatEnhancingDrugsChances[pawnKindDef.defName])
            {
                instance.Settings.CustomCombatEnhancingDrugsChances[pawnKindDef.defName] =
                    pawnKindDef.combatEnhancingDrugsChance;
                GUI.color = Color.green;
            }
            else
            {
                if (instance.Settings.CustomCombatEnhancingDrugsChances.ContainsKey(pawnKindDef.defName))
                {
                    instance.Settings.CustomCombatEnhancingDrugsChances.Remove(pawnKindDef.defName);
                }
            }

            pawnKindDef.combatEnhancingDrugsChance =
                (float)Math.Round((decimal)Widgets.HorizontalSlider(
                    sliderRect,
                    pawnKindDef.combatEnhancingDrugsChance, 0,
                    1f, false,
                    "CPS.percent".Translate(Math.Round(pawnKindDef.combatEnhancingDrugsChance * 100)),
                    pawnkindLabel,
                    modInfo), 2);

            GUI.color = Color.white;
        }

        scrollListing.End();

        Widgets.EndScrollView();
    }

    private void headgearScrollView(ref Rect frameRect)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label("CPS.headgear".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), "CPS.headgear.tooltip".Translate());

        if (instance.Settings.CustomHeadgearChances == null)
        {
            instance.Settings.CustomHeadgearChances = new Dictionary<string, float>();
        }

        if (instance.Settings.CustomHeadgearChances.Any())
        {
            DrawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate("CPS.headgear".Translate().ToLower()),
                        delegate { instance.Settings.ResetHeadgearValues(); }));
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
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack.Name.ToLower()
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

            var pawnkindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnkindLabel.Length > 45)
            {
                pawnkindLabel = $"{pawnkindLabel.Substring(0, 42)}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo.Substring(0, 42)}...";
            }

            if (pawnKindDef.apparelAllowHeadgearChance != Headgear.VanillaHeadgearChances[pawnKindDef.defName])
            {
                instance.Settings.CustomHeadgearChances[pawnKindDef.defName] =
                    pawnKindDef.apparelAllowHeadgearChance;
                GUI.color = Color.green;
            }
            else
            {
                if (instance.Settings.CustomHeadgearChances.ContainsKey(pawnKindDef.defName))
                {
                    instance.Settings.CustomHeadgearChances.Remove(pawnKindDef.defName);
                }
            }

            pawnKindDef.apparelAllowHeadgearChance =
                (float)Math.Round((decimal)Widgets.HorizontalSlider(
                    sliderRect,
                    pawnKindDef.apparelAllowHeadgearChance, 0,
                    1f, false,
                    "CPS.percent".Translate(Math.Round(pawnKindDef.apparelAllowHeadgearChance * 100)),
                    pawnkindLabel,
                    modInfo), 2);

            GUI.color = Color.white;
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    private void royalTitleChanceScrollView(ref Rect frameRect)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label("CPS.royaltitlechance".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), "CPS.royaltitlechance.tooltip".Translate());

        if (instance.Settings.CustomRoyalTitleChances == null)
        {
            instance.Settings.CustomRoyalTitleChances = new Dictionary<string, float>();
        }

        if (instance.Settings.CustomRoyalTitleChances.Any())
        {
            DrawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate("CPS.royalTitleChance".Translate().ToLower()),
                        delegate { instance.Settings.ResetRoyalTitleChanceValues(); }));
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
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack.Name.ToLower()
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

            var pawnkindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnkindLabel.Length > 45)
            {
                pawnkindLabel = $"{pawnkindLabel.Substring(0, 42)}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo.Substring(0, 42)}...";
            }

            if (pawnKindDef.royalTitleChance != RoyalTitleChance.RoyalTitleChances[pawnKindDef.defName])
            {
                instance.Settings.CustomRoyalTitleChances[pawnKindDef.defName] =
                    pawnKindDef.royalTitleChance;
                GUI.color = Color.green;
            }
            else
            {
                if (instance.Settings.CustomRoyalTitleChances.ContainsKey(pawnKindDef.defName))
                {
                    instance.Settings.CustomRoyalTitleChances.Remove(pawnKindDef.defName);
                }
            }

            pawnKindDef.royalTitleChance =
                (float)Math.Round((decimal)Widgets.HorizontalSlider(
                    sliderRect,
                    pawnKindDef.royalTitleChance, 0,
                    1f, false,
                    "CPS.percent".Translate(Math.Round(pawnKindDef.royalTitleChance * 100)),
                    pawnkindLabel,
                    modInfo), 2);

            GUI.color = Color.white;
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    private void techhediffsScrollView(ref Rect frameRect)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label("CPS.techhediffs".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), "CPS.techhediffs.tooltip".Translate());

        if (instance.Settings.CustomTechHediffsChances == null)
        {
            instance.Settings.CustomTechHediffsChances = new Dictionary<string, float>();
        }

        if (instance.Settings.CustomTechHediffsChances.Any())
        {
            DrawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate("CPS.techhediffs".Translate().ToLower()),
                        delegate { instance.Settings.ResetTechHediffsValues(); }));
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
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack.Name.ToLower()
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

            var pawnkindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnkindLabel.Length > 45)
            {
                pawnkindLabel = $"{pawnkindLabel.Substring(0, 42)}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo.Substring(0, 42)}...";
            }

            if (pawnKindDef.techHediffsChance !=
                TechHediffs.VanillaTechHediffsChances[pawnKindDef.defName])
            {
                instance.Settings.CustomTechHediffsChances[pawnKindDef.defName] =
                    pawnKindDef.techHediffsChance;
                GUI.color = Color.green;
            }
            else
            {
                if (instance.Settings.CustomTechHediffsChances.ContainsKey(pawnKindDef.defName))
                {
                    instance.Settings.CustomTechHediffsChances.Remove(pawnKindDef.defName);
                }
            }

            pawnKindDef.techHediffsChance =
                (float)Math.Round((decimal)Widgets.HorizontalSlider(
                    sliderRect,
                    pawnKindDef.techHediffsChance, 0,
                    1f, false,
                    "CPS.percent".Translate(Math.Round(pawnKindDef.techHediffsChance * 100)),
                    pawnkindLabel,
                    modInfo), 2);

            GUI.color = Color.white;
        }

        scrollListing.End();

        Widgets.EndScrollView();
    }

    private void techhediffsmoneyScrollView(ref Rect frameRect)
    {
        listing_Standard.Begin(frameRect);

        Text.Font = GameFont.Medium;

        var headerLabel = listing_Standard.Label("CPS.techhediffsmoney".Translate());
        TooltipHandler.TipRegion(new Rect(
            headerLabel.position,
            searchSize), "CPS.techhediffsmoney.tooltip".Translate());

        if (instance.Settings.CustomTechHediffsMoney == null)
        {
            instance.Settings.CustomTechHediffsMoney = new Dictionary<string, FloatRange>();
        }

        if (instance.Settings.CustomTechHediffsMoney.Any())
        {
            DrawButton(() =>
                {
                    Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                        "CPS.resetone.confirm".Translate("CPS.techhediffsmoney".Translate().ToLower()),
                        delegate { instance.Settings.ResetTechHediffsMoneyValues(); }));
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
                    def.label.ToLower().Contains(searchText.ToLower()) || def.modContentPack.Name.ToLower()
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
            alternate = !alternate;
            var sliderRect = scrollListing.GetRect(50);
            if (alternate)
            {
                Widgets.DrawBoxSolid(sliderRect, alternateBackground);
            }

            Text.Font = GameFont.Tiny;
            var modInfo = pawnKindDef.modContentPack?.Name;
            var pawnkindLabel = $"{pawnKindDef.label.CapitalizeFirst()} ({pawnKindDef.defName})";
            if (pawnkindLabel.Length > 45)
            {
                pawnkindLabel = $"{pawnkindLabel.Substring(0, 42)}...";
            }

            if (modInfo is { Length: > 45 })
            {
                modInfo = $"{modInfo.Substring(0, 42)}...";
            }

            if (pawnKindDef.techHediffsMoney !=
                TechHediffsMoney.VanillaTechHediffsMoney[pawnKindDef.defName])
            {
                instance.Settings.CustomTechHediffsMoney[pawnKindDef.defName] =
                    pawnKindDef.techHediffsMoney;
                GUI.color = Color.green;
            }
            else
            {
                if (instance.Settings.CustomTechHediffsMoney.ContainsKey(pawnKindDef.defName))
                {
                    instance.Settings.CustomTechHediffsMoney.Remove(pawnKindDef.defName);
                }
            }

            var smallerRect = sliderRect.ContractedBy(4f, 10f);
            smallerRect.y += 5;
            smallerRect.x += 2;

            Widgets.FloatRange(
                smallerRect,
                pawnKindDef.GetHashCode(),
                ref pawnKindDef.techHediffsMoney,
                0,
                8000,
                null,
                ToStringStyle.Money);
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.Label(smallerRect, pawnkindLabel);
            Text.Anchor = TextAnchor.UpperRight;
            Widgets.Label(smallerRect, modInfo);
            Text.Anchor = default;
            GUI.color = Color.white;
            scrollListing.Gap(10);
        }

        scrollListing.End();
        Widgets.EndScrollView();
    }

    private void DrawTabsList(Rect rect)
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

        var listAddition = 24;


        tabContentRect.height = (settingTabs.Count * 25f) + listAddition;
        Widgets.BeginScrollView(tabFrameRect, ref tabsScrollPosition, tabContentRect);
        listing_Standard.Begin(tabContentRect);
        //Text.Font = GameFont.Tiny;
        foreach (var settingTab in settingTabs)
        {
            if (string.IsNullOrEmpty(settingTab))
            {
                listing_Standard.ListItemSelectable(null, Color.yellow, out _);
                continue;
            }

            if (listing_Standard.ListItemSelectable($"CPS.{settingTab.ToLower()}".Translate(), Color.yellow,
                    out _, SelectedDef == settingTab))
            {
                SelectedDef = SelectedDef == settingTab ? null : settingTab;
            }
        }

        listing_Standard.End();
        //Text.Font = GameFont.Small;
        Widgets.EndScrollView();
    }
}