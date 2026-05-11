# ZonguldakGameJam - Core Loop/Shop Setup

## New Managers
- `Assets/Scripts/PersistentManager.cs`: Keeps money, fish count, current day, and permanent base stats across scenes.
- `Assets/Scripts/DayManager.cs`: Holds temporary multipliers and resets each day/scene.
- `Assets/Scripts/TownManager.cs`: Handles building triggers, prompts, and day end sequence.
- `Assets/Scripts/ShopUIManager.cs`: Handles button-driven purchases and warning UI.

## Scene Setup (Minimum)
1. Add `PersistentManager` to a boot scene GameObject (DontDestroyOnLoad).
2. Add `DayManager` to each day scene (not persistent).
3. Town building object:
   - Add three `BoxCollider2D` (IsTrigger = true).
   - Assign them to `TownManager` (sell, upgrade, home).
   - Set player tag to `Player`.
4. Hook-up UI:
   - Assign panels and prompts in `TownManager`.
   - Hook `ShopUIManager` to upgrade/buff canvases and wire buttons to its public methods.
5. Optional: fade overlay
   - Add a `CanvasGroup` and day text, assign to `TownManager`.

## Notes
- Fish sales use `fishCount * 10` from `PersistentManager`.
- Day reset happens on scene reload.

