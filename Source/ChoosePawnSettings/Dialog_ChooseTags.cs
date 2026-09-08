using System.Collections.Generic;
using System.Linq;
using ChoosePawnSettings.Settings;
using UnityEngine;
using Verse;

namespace ChoosePawnSettings;

public class Dialog_ChooseTags : Window
{
    private readonly string currentPawnKindDefName;
    private readonly Dictionary<string, List<ThingDef>> currentTagContents;
    private readonly List<string> originalTagContents;

    private Vector2 scrollPosition;

    public Dialog_ChooseTags(Dictionary<string, List<ThingDef>> tagContents, string pawnKindDefName)
    {
        scrollPosition = Vector2.zero;
        absorbInputAroundWindow = true;
        currentTagContents = tagContents;
        currentPawnKindDefName = pawnKindDefName;
        originalTagContents = ChoosePawnSettings_Mod.CurrentTags is null
            ? []
            : [..ChoosePawnSettings_Mod.CurrentTags];
    }


    public override void DoWindowContents(Rect inRect)
    {
        GUI.contentColor = Color.green;
        Widgets.Label(new Rect(inRect), "CPS.selecttags".Translate());
        GUI.contentColor = Color.white;
        Widgets.DrawLineHorizontal(inRect.x, inRect.y + 25f, inRect.width);

        if (Widgets.ButtonText(
                new Rect(inRect.position + new Vector2(inRect.width - ChoosePawnSettings_Mod.buttonSize.x, 0),
                    ChoosePawnSettings_Mod.buttonSize),
                "CPS.save.button".Translate()))
        {
            // If the tags are the same as the original, clear the current tags to avoid saving unnecessary data
            if (ChoosePawnSettings_Mod.CurrentTags != null &&
                ChoosePawnSettings_Mod.CurrentTags.SequenceEqual(originalTagContents))
            {
                ChoosePawnSettings_Mod.CurrentTags = null;
            }

            ChoosePawnSettings_Mod.TagStage = currentPawnKindDefName;
            Find.WindowStack.TryRemove(this, false);
            return;
        }

        if (Widgets.ButtonText(
                new Rect(inRect.position + new Vector2(inRect.width - (ChoosePawnSettings_Mod.buttonSize.x * 2), 0),
                    ChoosePawnSettings_Mod.buttonSize),
                "CPS.reset.button".Translate()))
        {
            ChoosePawnSettings_Mod.CurrentTags = null;
            ChoosePawnSettings_Mod.TagStage = currentPawnKindDefName;
            Find.WindowStack.TryRemove(this, false);
            return;
        }

        if (Widgets.ButtonText(
                new Rect(inRect.position + new Vector2(inRect.width - (ChoosePawnSettings_Mod.buttonSize.x * 2.5f), 0),
                    ChoosePawnSettings_Mod.buttonSize - new Vector2(ChoosePawnSettings_Mod.buttonSize.x * 0.5f, 0)),
                "CPS.all.button".Translate()))
        {
            foreach (var currentTagContent in currentTagContents.Select(pair => pair.Key))
            {
                if (ChoosePawnSettings_Mod.CurrentTags.Contains(currentTagContent))
                {
                    continue;
                }

                ChoosePawnSettings_Mod.CurrentTags.Add(currentTagContent);
            }

            return;
        }

        if (Widgets.ButtonText(
                new Rect(inRect.position + new Vector2(inRect.width - (ChoosePawnSettings_Mod.buttonSize.x * 3f), 0),
                    ChoosePawnSettings_Mod.buttonSize - new Vector2(ChoosePawnSettings_Mod.buttonSize.x * 0.5f, 0)),
                "CPS.none.button".Translate()))
        {
            foreach (var currentTagContent in currentTagContents.Select(pair => pair.Key))
            {
                if (!ChoosePawnSettings_Mod.CurrentTags.Contains(currentTagContent))
                {
                    continue;
                }

                ChoosePawnSettings_Mod.CurrentTags.Remove(currentTagContent);
            }

            return;
        }

        var viewRect = inRect;
        viewRect.y += 40f;
        viewRect.width -= 10f;
        viewRect.x += 5f;
        var contentRect = viewRect;
        contentRect.width -= 20;
        contentRect.x = 0;
        contentRect.y = 0f;
        contentRect.height = currentTagContents.Count * 26f;
        Widgets.BeginScrollView(viewRect, ref scrollPosition, contentRect);
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(contentRect);
        foreach (var tag in currentTagContents.OrderBy(pair => pair.Key))
        {
            var isChecked = ChoosePawnSettings_Mod.CurrentTags?.Contains(tag.Key) == true;
            var wasChecked = isChecked;
            var currentTagContent = tag.Value.OrderBy(s => s.label).Select(def => def.LabelCap);
            listingStandard.CheckboxLabeled($"{tag.Key} ({currentTagContent.Count()})", ref isChecked,
                string.Join("\n", currentTagContent));
            if (isChecked == wasChecked)
            {
                continue;
            }

            if (isChecked)
            {
                ChoosePawnSettings_Mod.CurrentTags ??= [];

                ChoosePawnSettings_Mod.CurrentTags.Add(tag.Key);
                break;
            }

            ChoosePawnSettings_Mod.CurrentTags?.Remove(tag.Key);
        }

        listingStandard.End();
        Widgets.EndScrollView();
    }
}