using System;
using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Pathfinding;

namespace HollowKnight.Enemies
{
public enum MantisLordState
{
    // TODO: Write States
    Idle,
    Throw,
    Death,
    DStabArrive
}

public class MantisLord : IEnemy
{
    public MantisLordState State { get; set; } = MantisLordState.DStabArrive;
    private Rectangle[] hitBoxes = new Rectangle[1];
    private readonly Dictionary<MantisLordState, ISprite> sprites;

    public bool Dead { get; set; }
    public Direction FacingDirection { get; set; } = Direction.Left;

    public int Health { get; set; } = EnemyConstants.EnemyDefaultHealth;
    public bool IsDamaged => _isDamaged;
    
    

    private bool _isDamaged;
    private double _damagedTimer;
    public bool IsGrounded { get; private set; } = false;

    public Vector2 knightPosition = new Vector2(GameConstants.InvalidPositionSentinel, GameConstants.InvalidPositionSentinel);

    public ISprite Sprite { get; private set; }
    public List<Vector2> GetCurrentPath() { return null; }
    public void SetNavigationGrid(NavigationGrid grid) { }
    public void SetKnightPosition(Vector2 knightPosition) => this.knightPosition = knightPosition;
    public float GetDetectionRadius() => 0f;
    public float GetChaseRadius() => 0f;
    public Vector2 position;

    public MantisLord(Vector2 position)
    {
        this.position = position;
        sprites = new Dictionary<MantisLordState, ISprite>
        {
            [MantisLordState.Idle] = SpriteFactory.Instance.CreateMantisThroneIdle(position),
            [MantisLordState.Throw] = SpriteFactory.Instance.CreateMantisThrow(position),
            [MantisLordState.Death] = SpriteFactory.Instance.CreateMantisDeath(position),
            [MantisLordState.DStabArrive] = SpriteFactory.Instance.CreateMantisDStabArrive(position)
            
        };
        Sprite = sprites[MantisLordState.DStabArrive];
        // stateMachine = new VengeflyStateMachine(this);
    }
    public bool TakeDamage() => TakeDamage(CollisionSide.None);
    public bool TakeDamage(CollisionSide side)
    {
        if (_isDamaged) return false;
        _isDamaged = true;
        _damagedTimer = 0;
        return true;
    }

    // public void SetState(VengeflyState newState)
    // {
    //     State = newState;
    //     Sprite = sprites[newState];
    // }

    public void SetPlatform(IObject platform) { } // Flying enemy doesn't need platform
    public bool IsActive => !Dead;
    public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, Sprite.Width, Sprite.Height);


    public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
    {
        SpriteEffects effects = FacingDirection == Direction.Right ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        Sprite.Draw(spriteBatch, effects);
    }

    public void Update(GameTime gameTime)
    {
        Sprite.SetPosition(position);
        Sprite.Update(gameTime);
    }

    public Rectangle[] GetBounds()
    {
        Vector2 size = Sprite.GetSize();
        hitBoxes[0] = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        return hitBoxes;
    }

    public Rectangle GetHurtbox()
    {
        Rectangle[] bounds = GetBounds();
        bounds[0].Inflate(EnemyConstants.EnemyHurtboxGrow, EnemyConstants.EnemyHurtboxGrow);
        return bounds[0];
    }

    public string GetStateName() => "DStabArrive";
}
}