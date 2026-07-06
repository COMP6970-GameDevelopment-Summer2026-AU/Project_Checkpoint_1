# Graveyard Keeper: Night Harvest — Checkpoint 1

**Course:** COMP 6910 · Game Development
**Engine:** Unity (URP) · third-person 3D
**Modules covered:** M6 (Building 3D Worlds) + M7 (Harvesting & Player Interaction)

> You are the night keeper of an old graveyard. Before dawn, work the grounds:
> chop wood, mine stone, and gather pumpkins scattered among the graves while
> ghosts drift between the tombstones and the sky slowly turns toward morning.

This is an original project that uses the same skills taught in Modules 6–7
(terrain world, third-person player + camera, day/night, minimap, compass,
scattered harvestable resources, interaction prompt, resource counters, audio)
but with its **own theme and assets** — a graveyard survival/harvesting game
rather than the wilderness forest built in class.

---

## Game Concept

A calm-but-spooky third-person harvesting game set in a fenced graveyard at night.
The keeper explores the grounds and harvests three resource types from objects
scattered across the terrain. A day/night cycle pushes the sky from moonlight
toward dawn, a minimap and compass help you navigate, and wandering ghosts give
the yard some life. Reach the harvest goals before dawn to restore the grounds.

## Current Gameplay Objective

Harvest enough of each resource before the night timer runs out:

- **Wood** — chop pines and fallen trunks.
- **Stone** — mine rock piles and broken gravestone debris.
- **Pumpkins** — collect pumpkins among the graves.

Meet all three targets → **Grounds Restored** (win). If dawn arrives first, the
night ends and you can play again.

## Controls

| Action | Input |
| --- | --- |
| Move | `W A S D` or **Arrow keys** (camera-relative) |
| Look / orbit camera | Mouse |
| Sprint | `Left Shift` |
| Harvest / interact | `E` (when the prompt appears) |
| Attack / banish ghosts | `Left mouse` |
| Zoom camera | Mouse scroll |
| Free the cursor | `Esc` |
| Play again (after night ends) | `Space` |
| Browse all 47 animations | `N` / `B` next / prev, `L` resume |

---

## How to Run (first time)

This package is a set of **Assets** to drop into a Unity URP project.

1. Create a new **Unity 3D (URP)** project (or use an existing URP project).
2. Make sure the **Input System** package is installed
   (*Window ▸ Package Manager ▸ Input System*). If Unity asks to enable the new
   backends, choose **Yes** (or set *Edit ▸ Project Settings ▸ Player ▸ Active
   Input Handling* to **Both**). The scripts also compile with the legacy input
   manager, so either setting works.
3. Import **TMP Essentials**: *Window ▸ TextMeshPro ▸ Import TMP Essential
   Resources* (needed for the on-screen text).
4. Copy the `Assets/` folder from this package into your project's `Assets/`.
   Let Unity import the models and audio.
5. Run the builder: **Tools ▸ Graveyard Keeper ▸ Build World (Checkpoint 1)**.
   It creates the whole scene at `Assets/Scenes/Graveyard.unity` and adds it to
   Build Settings.
6. Press **Play**.

> The builder is also the **M7 "custom scatter tool"** — it procedurally lays out
> the terrain, fence, decoration, and all harvestable resource nodes, and wires
> up the player, camera, HUD, minimap, and compass in one click. Re-run it any
> time to regenerate a fresh layout.

---

## What's Working in Checkpoint 1

Mapped to the checkpoint requirements:

| # | Requirement | In this build |
| --- | --- | --- |
| 1 | Original 3D project | Graveyard Keeper (own theme + assets) |
| 2 | Playable 3D world | Unity **Terrain** with height noise, fenced graveyard, scattered props |
| 3 | Controllable player | Third-person keeper — `ThirdPersonController.cs` (CharacterController) |
| 4 | Working camera | Orbit-follow camera with zoom + collision — `CameraRig.cs` |
| 5 | 3+ interactive objects | Wood / Stone / Pumpkin harvest nodes (30+ placed) + ghosts |
| 6 | Interaction system | Proximity detection + `E` to harvest — `PlayerInteractor.cs`, `Harvestable.cs` |
| 7 | UI feedback | Resource counters, objective, night timer, interaction prompt, end panel |
| 8 | Audio | Kenney UI SFX for harvest/collect + procedural night ambience — `AudioManager.cs` |
| 9 | Own design choices | Graveyard theme, day/night cycle, minimap, compass, wandering ghosts |
| 10 | Runs without major errors | Full loop: play → harvest → night ends → restart |

**Module 6 skills:** Unity Terrain world, third-person player + camera, day/night
cycle (`DayNightCycle.cs`), minimap (`MinimapFollow.cs` + render texture), compass
(`Compass.cs`), and audio.

**Module 7 skills:** a custom Editor **scatter tool** (`GraveyardWorldBuilder.cs`),
harvestable resource nodes placed as individual GameObjects, an **interaction
prompt** and **resource counters**, with distinct verbs/feedback per resource type
(Chop / Mine / Collect). *(Kenney models are static, so per-type harvest feedback
is a scale-punch + distinct sound; this is where Mixamo harvest animations would
plug in for the final version.)*

## My Own Design Choices

- Graveyard theme and setting instead of the class's forest.
- **Endless world:** the graveyard streams around the player on a grid — graves,
  resources, and ghosts spawn ahead and recycle behind, so it never ends
  (`EndlessWorld.cs`).
- **Combat:** left-click swings the axe; ghosts flash, get knocked back, and are
  banished after enough hits, and they drift toward you when you're close
  (`CombatController.cs` + `GhostWander.cs`). A "Spirits banished" counter tracks
  your kills.
- Three themed resources (Wood / Stone / Pumpkin) with per-type interaction verbs.
- Night timer + win state ("Grounds Restored").
- Day/night cycle that eases lighting and fog from moonlight toward dawn.
- **Night sky:** a skybox plus a world-fixed **moon** (procedurally textured) and a
  scatter of stars that dim toward dawn (`MoonSky.cs`).
- **Ghost voices:** synthesized moans, whispers, and wails play at the start of the
  night, at the midpoint, and randomly every ~20–40s (`GhostVoiceDirector.cs` +
  `AudioManager.cs`). Drop your own `.ogg`/`.wav` files into
  `Assets/Resources/GKAudio/Voices/` to replace them.
- Minimap and rotating compass for navigation.
- A fully self-contained audio setup: Kenney UI sounds for interactions and combat
  plus a procedurally generated wind-and-crickets ambience and axe-swing whoosh.
- An animated hero via the Pro Melee Axe Pack, with an in-game browser for all 47
  animations.

---

## External Assets & Resources

- **Kenney — Graveyard Kit (5.0)** — 3D models (keeper, ghost, gravestones,
  crypts, fences, pines, pumpkins, rocks, etc.). License: **CC0**.
  https://kenney.nl/assets  (see `Assets/Art/Graveyard/Kenney-Graveyard-License.txt`)
- **Kenney — UI Audio (UI SFX Set)** — click / switch / rollover interaction
  sounds. License: **CC0**. https://kenney.nl/assets/ui-audio
  (see `Assets/Resources/GKAudio/Kenney-UIAudio-License.txt`)
- **Unity Technologies** — Universal Render Pipeline (URP), TextMeshPro, and the
  Input System packages.
- **Night ambience** — generated procedurally at runtime in `AudioManager.cs`
  (original). Optional future swap: free nature/ambience tracks from Pixabay
  (as referenced in the Module 6 resources).

All third-party assets used here are CC0 and free for educational use.

---

## Scripts

| Script | Role |
| --- | --- |
| `GKInput.cs` | Input helper (new Input System or legacy) |
| `ThirdPersonController.cs` | Keeper movement (camera-relative, sprint, gravity) |
| `CameraRig.cs` | Orbit-follow third-person camera |
| `Harvestable.cs` | Resource node: type, hits, yield, feedback |
| `PlayerInteractor.cs` | Nearest-node detection, prompt, harvest on `E` |
| `GraveyardManager.cs` | Resource counts, objective, timer, HUD, win/restart |
| `AudioManager.cs` | Kenney UI SFX + procedural night ambience |
| `DayNightCycle.cs` | Rotates the moon/sun, eases fog & ambient |
| `Compass.cs` | Rotating compass needle + heading |
| `MinimapFollow.cs` | Top-down minimap camera follower |
| `GhostWander.cs` | Roaming ghost with combat reactions (flash / knockback / banish) |
| `CombatController.cs` | Left-click melee that hits ghosts in a forward arc |
| `EndlessWorld.cs` | Streams graves/resources/ghosts endlessly around the player |
| `MainMenuController.cs` | Title screen — PLAY / Enter starts the game |
| `Editor/MainMenuBuilder.cs` | Builds the title-screen scene from the artwork |
| `KeeperAnimator.cs` | Drives locomotion + harvest + attack animations |
| `AnimationShowcase.cs` | Cycle/play all 47 axe-pack animations (N/B/L) |
| `Editor/GraveyardWorldBuilder.cs` | One-click world + scatter tool |
| `Editor/KeeperAnimatorBuilder.cs` | Animator Controller from generic Mixamo clips |
| `Editor/AxePackImporter.cs` | Auto-imports the axe pack as Humanoid |
| `Editor/AxePackSetup.cs` | One-click: The Boss character + 47-anim controller |

> **Animated character (recommended):** the project ships with the **Pro Melee
> Axe Pack** — a rigged character ("The Boss") and 47 animations. After building
> the world, run *Tools ▸ Graveyard Keeper ▸ Setup Axe Pack Character (47 anims)*
> to swap it in and wire everything. See `AXEPACK_SETUP.md`. In play mode: WASD
> to move, E to harvest (axe attacks), and N/B/L to browse all 47 animations.
>
> To use a different Mixamo character instead, see `MIXAMO_SETUP.md`.

---

*Checkpoint 1 — an in-progress foundation, not the finished game. This README will
keep being updated for the final submission.*
