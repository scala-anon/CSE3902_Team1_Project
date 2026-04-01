using HollowKnight.Shared;

namespace HollowKnight;

public partial class Game1
{
    public void TogglePause()
    {
        if (_gameState == GameState.Playing)
        {
            _gameState = GameState.Paused;
        }
        else if (_gameState == GameState.Paused)
        {
            _gameState = GameState.Playing;
        }
    }

    public void ToggleInventory()
    {
        if (_gameState == GameState.Playing)
        {
            _gameState = GameState.Inventory;
        }
        else if (_gameState == GameState.Inventory)
        {
            _gameState = GameState.Playing;
        }
    }

    public void SetGameOver()
    {
        _gameState = GameState.GameOver;
    }

    public void SetWin()
    {
        _gameState = GameState.Win;
    }

    public void SetPlaying()
    {
        _gameState = GameState.Playing;
    }

    public void SwitchToNextRoom()
    {
        _roomManager.SwitchRoomByOffset(1);
    }

    public void SwitchToPreviousRoom()
    {
        _roomManager.SwitchRoomByOffset(-1);
    }
}