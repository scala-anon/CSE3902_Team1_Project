using Microsoft.Xna.Framework;
using HollowKnight.Interfaces;
using HollowKnight.Factories;
using System.Collections.Generic;
using HollowKnight.Player;

namespace HollowKnight.Player
{
    public static class KnightSpriteBuilder
    {
        public static Dictionary<KnightSpriteType, ISprite> BuildKnightSprites(Vector2 position)
        {
            return new Dictionary<KnightSpriteType, ISprite>
            {
                [KnightSpriteType.Idle] = SpriteFactory.Instance.CreateKnightIdleSprite(position),
                [KnightSpriteType.Walking] = SpriteFactory.Instance.CreateKnightWalkSprite(position),
                [KnightSpriteType.Jumping] = SpriteFactory.Instance.CreateKnightJumpSprite(position),
                [KnightSpriteType.SideSlash] = SpriteFactory.Instance.CreateKnightSideSwordSprite(position),
                [KnightSpriteType.UpSlash] = SpriteFactory.Instance.CreateKnightUpSwordSprite(position),
                [KnightSpriteType.DownSlash] = SpriteFactory.Instance.CreateKnightDownSwordSprite(position),
                [KnightSpriteType.Damaged] = SpriteFactory.Instance.CreateKnightDamagedSprite(position),
                [KnightSpriteType.HealPrep] = SpriteFactory.Instance.CreateKnightHealPrepSprite(position),
                [KnightSpriteType.HealPost] = SpriteFactory.Instance.CreateKnightHealPostSprite(position),
                [KnightSpriteType.SpiritCast] = SpriteFactory.Instance.CreateSpiritCastSprite(position),
                [KnightSpriteType.Dashing] = SpriteFactory.Instance.CreateKnightDashSprite(position),

            };
        }
    }
}