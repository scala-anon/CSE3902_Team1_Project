using Microsoft.Xna.Framework;
using HollowKnight.Collision;
using HollowKnight.Shared;

namespace HollowKnight.Player
{
  public class SwordHitbox : ICollidable
  {
    private Rectangle _hitbox;

    public SwordHitbox(Rectangle knightBounds, Direction facing, KnightSpriteType attackType)
    {
      _hitbox = ComputeBounds(knightBounds, facing, attackType);  
    }

    public Rectangle Bounds => _hitbox;
    public bool IsActive => true;
    public Rectangle GetBounds() => _hitbox;

    //TODO: fix the values to fit sword frames
    private static Rectangle ComputeBounds(Rectangle knightBounds, Direction facing, KnightSpriteType attackType)
    {
      int width = 0;
      int height = 0;
      int xOffset = 0;
      int yOffset = 0;

      // TODO: updated hardcoded values 
      switch (attackType)
      {
        case KnightSpriteType.SideSlash:
          width = 60;
          height = 90;
          xOffset = facing == Direction.Right ? knightBounds.Width : -width;
          yOffset = (knightBounds.Height-height)/2;
          break;
        case KnightSpriteType.UpSlash:
          width = 75;
          height = 75;
          xOffset = (knightBounds.Width - width) / 2;
          yOffset = -height;
          break;
        case KnightSpriteType.DownSlash:
          width = 75;
          height = 75;
          xOffset = (knightBounds.Width - width) / 2;
          yOffset = knightBounds.Height;
          break;
      }

      return new Rectangle(knightBounds.X + xOffset, knightBounds.Y + yOffset, width, height);
    }
  }
}