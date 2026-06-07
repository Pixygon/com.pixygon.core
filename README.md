# Pixygon — Core

Foundational runtime for a Pixygon app: what an "experience" is, how it boots and
loads scenes, pausing, the interaction contract, and small shared primitives
(tools, rarity, notifications). Most other packages and games depend on this.

## Overview

A Pixygon game/app is an **Experience** (`ExperienceData`) with a version, a main
scene, linked scenes, and tablet-app metadata. Core provides the boot/loader,
global pause, and the `IInteractable` contract that world objects implement.

## Key types

| Type | What it is |
|---|---|
| **`ExperienceData`** | `ScriptableObject` describing an app: `Major.Minor.Patch.Build` version, `MainScene`, `LinkedScenes`, `TabletAppDatas`. |
| **`ExperienceSettings` / `SceneSettings` / `SceneData` / `TabletAppData`** | Scene + app configuration. |
| **`Bootstrap` / `GameLoader`** | Boot entry + scene loading. |
| **`PauseManager`** (+ `PauseToggler`, `TimedToggler`) | Global pause state; gameplay polls it. |
| **`IInteractable`** (+ `Tool`) | Interaction contract: `RequiredTool`/`RequiredToolLevel`, `InteractAction`, `InteractSprite`, `Interact(tool, level)`. |
| **`Rarity`** | Shared rarity scale (used by e.g. `micro.SkinCard`). |
| **`Notifications`** | In-game notification helper. |
| **`LevelObject` / `SubsystemModule` / `HashGenerator`** | Small shared utilities. |
| **`PixygonAPI`** | ⚠️ A second API type — see note. |

## Dependencies

None declared.

## Usage

```csharp
public class Chest : MonoBehaviour, Pixygon.Core.IInteractable {
    public Tool RequiredTool => null;
    public string InteractAction => "Open";
    public void Interact(Tool tool, int level) { /* … */ }
}
if (Pixygon.Core.PauseManager.Paused) return; // gameplay pauses uniformly
```

## Status

`0.5.0`. ⚠️ **Known cleanup:** there are two API types in the platform —
`Pixygon.Core.PixygonAPI` (here) and `Pixygon.Passport.PixygonApi` (the account/login
API). Clarify which is canonical and dedupe (see `PIXYGON_ENGINE_HANDOFF.md`).
Engine-portability: extract pure-C# cores for the logic pieces (pause, experience
model) when moving off Unity.
