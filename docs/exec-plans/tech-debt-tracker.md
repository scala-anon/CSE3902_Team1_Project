# Tech Debt Tracker

## Rendering / Depth Ordering

- [ ] **Wire LayerDepthBackgroundMid / LayerDepthBackgroundRocks when parallax backgrounds land.** Constants already exist in `HollowKnight/Shared/GameConstants.cs` but are unreferenced. Trigger: first parallax background PR. Source: sprite-depth-ordering refactor follow-up (dev @ 4ff87ca).
- [ ] **Plumb layerDepth through TextSprite.Draw to the 9-arg DrawString overload if world-space text is ever added.** `HollowKnight/Sprites/TextSprite.cs` currently accepts a `layerDepth` parameter and silently drops it. Trigger: first caller of `SpriteFactory.CreateTextSprite` (currently zero call sites). Source: sprite-depth-ordering refactor follow-up (dev @ 4ff87ca).

## Enemies

- [ ] **Crawlid knockback still not working.** `aec7d61` on `feature/room-visual-assets` attempted a fix but the knockback behavior remains broken. Trigger: revisit when polishing enemy feel. Source: feature/room-visual-assets PR.
