# GitHub Copilot Instructions for Choose Pawn Settings Mod

## Mod Overview and Purpose

**Mod Name:** Choose Pawn Settings  
**Mod Description:** The Choose Pawn Settings mod allows players to modify various attributes of pawns in RimWorld, enhancing gameplay customization. Originally intended as a replacement for Configurable Biocoding, this mod expanded to include a broader range of properties that can be adjusted to tailor pawn behavior and attributes to player preferences.

The modifications apply immediately, although they only affect pawns that spawn after changes are made. Suggestions for additional adjustable properties are welcomed by the mod author.

## Key Features and Systems

### Implemented Features:
- **Biocode Chance:** Control the likelihood of weapons being biocoded.
- **Chemical Addiction Chance:** Adjust how often pawns develop chemical addictions.
- **Combat Enhancing Drugs Chance:** Set the probability of pawns using combat drugs.
- **Headgear Chance:** Determine the chance that pawns will wear headgear.
- **Combat Power:** Modify the combat power rating of pawns.
- **Tech Hediffs Chance, Money, and Tags:** Manage technological health differences.
- **Weapon Money and Tags:** Customize the monetary value and types of weapons available to pawns.
- **Apparel Money and Tags:** Configure the cost and categories of wearable apparel.
- **Generation Age:** Alter the age at which pawns are generated.
- **Gender Probability:** Fine-tune the likelihood of a pawn being a specific gender.
- **Death Acidifier:** Enable or disable death acidifiers in pawns.
- **Royal Title Chance (Royalty required):** Adjust the chance of pawns receiving royal titles if the Royalty DLC is enabled.

## Coding Patterns and Conventions

- **Naming Conventions:** Classes and methods follow PascalCase, while variables use camelCase for clarity and consistency.
- **Methodology:** Classes often contain `Initialize` methods to set the mod options initially. Methods like `ResetToVanillaValues` and `SetCustomValues` are used to toggle between modded settings and defaults.
- **Documentation:** Comments and method summaries help explain the purpose of major logic sections and configurations.

## XML Integration

Although the mod, as currently structured, does not directly modify XML defs, integration may be necessary if the mod expands to include XML changes. For future XML handling, consider using:
- **PatchOperations:** To make non-destructive changes to existing game XML files.
- **XML Defs File Creation:** If new defs are introduced.

## Harmony Patching

The Choose Pawn Settings mod utilizes **Harmony** for runtime modifications without altering RimWorld's assemblies, ensuring compatibility with the base game and other mods.

- **Namespace:** Ensure that all Harmony patches are wrapped with appropriately scoped namespaces for clarity.
- **Patch Methodology:** Use `Prefix`, `Postfix`, and `Transpiler` where suitable, with detailed comments explaining the purpose of each patch.
- **Dependency Management:** Declare the dependency on Harmony via `brrainz.harmony` in the mod's configuration files.

## Suggestions for Copilot

Given the structure of this project, GitHub Copilot could assist by:
- **Generating Method Stubs:** Automate the creation of initialization, reset, and set methods for new customizable attributes in the mod.
- **Code Documentation:** Provide comments and summaries for new methods and classes to ensure they align with existing project documentation practices.
- **Harmony Patch Templates:** Suggest Transpiler patches when adding new game logic interventions.
- **Error Handling Patterns:** Offer robust error handling constructs to improve mod stability.
- **Unit Testing Assistance:** If considering tests, Copilot can assist in generating basic unit test structures using a testing framework compatible with RimWorld mods.

By following these guidelines and leveraging the power of GitHub Copilot, further development on the Choose Pawn Settings mod can be efficient and maintainable.

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

