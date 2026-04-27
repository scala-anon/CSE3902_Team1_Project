using HollowKnight.Shared;
using Microsoft.Xna.Framework;
using System;

namespace HollowKnight;

public partial class Game1
{
    public void TogglePause()
    {
        if (_gameState is PlayingState)
        {
            _gameState = new PausedState();
        }
        else if (_gameState is PausedState)
        {
            _gameState = new PlayingState();
        }
    }

    public void ToggleInventory()
    {
        if (_gameState is PlayingState)
        {
            _gameState = new InventoryState();
        }
        else if (_gameState is InventoryState)
        {
            _gameState = new PlayingState();
        }
    }

    public void SetWin()
    {
        _gameState = new WinState();
    }

    public void SetPlaying()
    {
        _gameState = new PlayingState();
    }

    public bool AllowsGameplayInput()
    {
        return _gameState is PlayingState;
    }

    public void StartMantisFight()
    {
        _level.BossFight?.Activate();
    }

    public bool IsInventoryOpen()
    {
        return _gameState is InventoryState;
    }

    public void ResetGame()
    {
        _restartRequested = true;
    }

    public void SwitchToNextRoom()
    {
        TransitionToRoom(_currentRoom + 1);
    }

    public void SwitchToPreviousRoom()
    {
        TransitionToRoom(_currentRoom - 1);
    }

    // Quick in-world jump by one screen-width (used by mouse-click navigation).
    // Does NOT trigger a room transition — just teleports the knight in world coords.
    public void QuickNavigateHorizontally(int direction)
    {
        if (direction == 0) return;

        int screenWidth = _graphics.PreferredBackBufferWidth;
        Vector2 pos = _knight.GetPosition();
        float newX = pos.X + direction * screenWidth;
        newX = Math.Clamp(newX, 0, GameConstants.DefaultLevelWidth - screenWidth);

        _knight.SetPosition(new Vector2(newX, pos.Y));
        Camera.Instance.SnapTo(_knight.GetPosition());
    }
}
