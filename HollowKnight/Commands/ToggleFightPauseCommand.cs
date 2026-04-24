using HollowKnight.Enemies;
using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class ToggleFightPauseCommand : ICommand
    {
        private readonly BossFightController _bossFight;

        public ToggleFightPauseCommand(BossFightController bossFight)
        {
            _bossFight = bossFight;
        }

        public void Execute() => _bossFight.TogglePause();
    }
}