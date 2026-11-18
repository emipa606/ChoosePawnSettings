# GitHub Copilot Instructions for ChoosePawnSettings Mod

Welcome to the ChoosePawnSettings mod for RimWorld! This mod allows you to customize various aspects of pawn kinds, enhancing the diversity and complexity of your RimWorld game.

## Mod Overview and Purpose

ChoosePawnSettings is a mod that lets you change various attributes and properties of pawn kinds in RimWorld. It was initially developed as a replacement for the Configurable Biocoding mod but has since expanded to include more properties for customization. The mod is designed to provide players with greater control over pawn generation, making their RimWorld experience more unique and tailored to their preferences.

## Key Features and Systems

ChoosePawnSettings currently supports the following customization options:

- **Biocode Chance**: Adjust the likelihood of a pawn kind having biocode.
- **Chemical Addiction Chance**: Modify the chance of a pawn kind being addicted to chemicals.
- **Combat Enhancing Drugs Chance**: Set the probability of a pawn kind using combat-enhancing drugs.
- **Headgear Chance**: Control the likelihood of a pawn kind wearing headgear.
- **Combat Power**: Adjust the base combat power of a pawn kind.
- **Tech Hediffs Chance**: Modify the chance of a pawn kind having tech hediffs.
- **Tech Hediffs Money**: Control the cost of tech hediffs for a pawn kind.
- **Tech Hediffs Tags**: Specify tags for tech hediffs associated with a pawn kind.
- **Weapon Money**: Adjust the cost of weapons for a pawn kind.
- **Weapon Tags**: Specify tags for weapons associated with a pawn kind.
- **Apparel Money**: Control the cost of apparel for a pawn kind.
- **Apparel Tags**: Specify tags for apparel associated with a pawn kind.
- **Generation Age**: Modify the age range for pawn kind generation.
- **Gender Probability**: Adjust the gender distribution for a pawn kind.
- **Death Acidifier**: Enable or disable the death acidifier effect for a pawn kind.
- **Royal Title Chance**: Modify the chance of a pawn kind having a royal title (requires Royalty mod).
- **Human Pregnancy Chance**: Control the chance of a pawn kind being able to have human pregnancies (requires Biotech mod).

All changes made through the mod are instant and will only affect pawns that are generated after the changes are applied.

## Coding Patterns and Conventions

### C# Naming Conventions

- **Classes**: PascalCase (e.g., `ChemicalAddiction`)
- **Methods**: PascalCase (e.g., `Initialize`)
- **Variables**: camelCase (e.g., `vanillaChemicalAddictionValues`)

### XML Naming Conventions

- **Tags**: Lowercase (e.g., `<pawnkind>`)
- **Attributes**: Lowercase (e.g., `id="example"`)
- **Values**: Lowercase or as specified in the mod documentation

### General Coding Guidelines

- Use meaningful and descriptive names for classes, methods, and variables.
- Follow the SOLID principles to ensure code is maintainable and extensible.
- Document code thoroughly, especially for public methods and classes.
- Ensure that all code is compatible with the latest version of RimWorld and the Harmony patching framework.

## XML Integration

ChoosePawnSettings does not currently use XML files for configuration. However, the mod is designed to be extensible, and future updates may include XML support for more advanced customization options. If you have suggestions for XML-based features, please leave a comment in the mod's discussion thread.

## Harmony Patching

ChoosePawnSettings uses the Harmony patching framework to modify the behavior of RimWorld. Harmony is a popular and widely-used tool for modders to make non-invasive changes to the game without modifying the original codebase. If you are unfamiliar with Harmony patching, you can find more information in the [Harmony documentation](https://harmony.pardeike.net/).

Here is an example of a Harmony patch used in ChoosePawnSettings:

csharp
using HarmonyLib;
using RimWorld;
using Verse;

namespace ChoosePawnSettings
{
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new Type[] { typeof(PawnGenerationRequest) })]
    public static class PawnGenerator_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn)
        {
            // Custom logic to modify pawn properties
            if (pawn.kindDef == PawnKindDef.Named("CustomKind"))
            {
                // Modify pawn properties
            }
        }
    }
}


## Suggestions for Copilot

To make the most of GitHub Copilot with the ChoosePawnSettings mod, consider the following suggestions:

- **Explore Harmony Patching**: If you are new to Harmony patching, start by exploring how it works in the mod and experiment with creating your own patches.
- **Understand XML Configuration**: If XML support is added in the future, familiarize yourself with the XML tags and attributes used in ChoosePawnSettings.
- **Contribute to the Mod**: If you have ideas for new features or improvements, consider contributing to the mod. Fork the repository and submit pull requests with your changes.
- **Document Your Code**: Ensure that your code is well-documented, especially if you are contributing new features or patches.

By following these guidelines, you can effectively use GitHub Copilot to enhance your RimWorld modding experience with ChoosePawnSettings.

Happy modding!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

