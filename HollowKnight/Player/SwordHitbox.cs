using Microsoft.Xna.Framework;
using HollowKnight.Collision;
using HollowKnight.Shared;

namespace HollowKnight.Player
{
  public class SwordHitbox : ICollidable
  {
    
    private Rectangle[] _hitbox = new Rectangle[1];
    

    public SwordHitbox(Rectangle knightBounds, Direction facing, KnightSpriteType attackType)
    {
      _hitbox[0] = ComputeBounds(knightBounds, facing, attackType);  
    }

    public Rectangle Bounds => _hitbox[0];
    public bool IsActive => true;
    public Rectangle[] GetBounds() => _hitbox;

   
    // TODO: Tune these dimensions to match sword sprite frames visually
    private static Rectangle ComputeBounds(Rectangle knightBounds, Direction facing, KnightSpriteType attackType)
    {
      int width = 0;
      int height = 0;
      int xOffset = 0;
      int yOffset = 0;

      switch (attackType)
      {
        case KnightSpriteType.SideSlash:
          width = CollisionConstants.SideSlashWidth;
          height = CollisionConstants.SideSlashHeight;
          xOffset = facing == Direction.Right ? knightBounds.Width : -width;
          yOffset = (knightBounds.Height-height)/2;
          break;
        case KnightSpriteType.UpSlash:
          width = CollisionConstants.UpSlashWidth;
          height = CollisionConstants.UpSlashHeight;
          xOffset = (knightBounds.Width - width) / 2;
          yOffset = -height;
          break;
        case KnightSpriteType.DownSlash:
          width = CollisionConstants.DownSlashWidth;
          height = CollisionConstants.DownSlashHeight;
          xOffset = (knightBounds.Width - width) / 2;
          yOffset = knightBounds.Height;
          break;
      }

      return new Rectangle(knightBounds.X + xOffset, knightBounds.Y + yOffset, width, height);
    }
  }
}