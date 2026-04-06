using System;
using HollowKnight.Shared;

namespace HollowKnight.Player
{
    public class KnightSoul
    {
        public int Soul { get; private set; } = GameConstants.KnightStartSoul;
        public int MaxSoul { get; } = GameConstants.KnightMaxSoul;

        public void AddSoul(int amount)
        {
            Soul = Math.Clamp(Soul + amount, 0, MaxSoul);
        }

        public float GetFillRatio()
        {
            if (MaxSoul <= 0)
            {
                return 0f;
            }

            return Soul / (float)MaxSoul;
        }
    }
}
