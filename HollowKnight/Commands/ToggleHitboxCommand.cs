using HollowKnight.Collision;
using HollowKnight.Interfaces;

namespace HollowKnight.Commands
{
    public class ToggleHitboxesCommand : ICommand
    {
        public ToggleHitboxesCommand(Game1 game) { }

        public void Execute()
        {
            DebugRenderer.hitboxEnabled = !DebugRenderer.hitboxEnabled;
        }
    }
}
