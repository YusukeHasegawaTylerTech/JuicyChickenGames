# JuicyChickenGames bootstrap

A small set of reusable Unity utilities meant to be copied into a new project's
`Assets/` folder as a starting point.

## Reusing this in a new project

Copy the whole `JuicyChickenGames/` folder (including the `.meta` files) into the
new project's `Assets/`. If the project doesn't need grid pathfinding or maze
generation, delete the `Pathing/` folder entirely — it's a self-contained,
Unity-agnostic module with its own assembly definition and nothing else in this
repo depends on it.

There's no package manifest/versioning here by design (see prior discussion on
keeping this Assets-based instead of a UPM package) — once copied into a project,
that project's copy is expected to be edited freely and will drift from this one.

## Layout

- **`Scripts/`** (`JuicyChickenGames.Runtime.asmdef`, namespace `JuicyChickenGames`)
  - `Audio/AudioManager.cs` — plays one-shot clips through a pool of `AudioSource`s.
  - `Audio/VolumeSlider.cs` — wires a UI `Slider` to an `AudioMixer` exposed
    parameter, converting between linear slider value and decibels.
  - `Input/SwipeDetector.cs` — detects a 4-directional swipe and raises the
    static `SwipeDetector.OnSwipe` event once per gesture. Assign `Camera` in
    the Inspector, or leave it blank to fall back to `Camera.main`.
  - `Scene/LoadingSceneIntegration.cs` — Editor-only. Lets you press Play on any
    scene and still go through your bootstrap flow: if the active scene isn't
    build index 0, it redirects to scene 0 and remembers which scene you meant
    to open. Have your bootstrap scene's script call
    `LoadingSceneIntegration.TryLoadOtherScene()` once it's done initializing;
    it loads that scene and returns `true`, or returns `false` (does nothing)
    if there's no scene to return to. Requires your bootstrap scene to be
    build index 0 in File > Build Settings > Scenes In Build.
  - `ScaleToFitScreen.cs` — scales a `SpriteRenderer` to fill an orthographic
    camera's viewport.
  - `Utilities/` — general extension methods (`EnumExtensions`,
    `IEnumerableExtensions`, `StringExtensions`, `TransformExtensions`,
    `UiExtensions`, `ReflectionExtensions`).
  - `Editor/AssetUtilities.cs` (`JuicyChickenGames.Editor.asmdef`, Editor-only,
    namespace `JuicyChickenGames.Editor`) — `EnsureAssetExists<T>` creates a
    `ScriptableObject` asset at a path if one doesn't already exist there.

- **`Pathing/`** (`JuicyChickenGames.Pathing.asmdef`, `noEngineReferences: true`,
  namespace `JuicyChickenGames.Pathing`) — optional, plain C#, no UnityEngine
  dependency at all.
  - `AStar.cs` — grid A* with 8-directional movement and an optional
    `canWalkTo` callback for custom traversal rules (e.g. blocking diagonal
    corner-cutting).
  - `BFS.cs` — grid breadth-first search to the first node matching a predicate.
  - `MazeGenerator.cs` — recursive-backtracker maze generator.

## Known limitations (not fixed, by design)

- `SwipeDetector.OnSwipe` is a `static` event, so only one swipe zone's worth of
  gestures should be active at a time. A project needing several independent
  swipe zones should switch this to an instance event.
- `AStar`'s open set is a linear-scanned `List<Node>`, not a priority queue —
  fine for small grids, but it won't scale to large maps.
- `MazeGenerator.GeneratePath` is recursive with no depth guard — very large
  mazes risk a stack overflow.
