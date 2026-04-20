using HollowKnight.Shared;

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

    public void SetGameOver()
    {
        _gameState = new GameOverState();
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

    public void ResetGame()
    {
        _restartRequested = true;
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
