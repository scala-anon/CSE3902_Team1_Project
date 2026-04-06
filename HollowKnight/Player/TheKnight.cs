using System;
using System.Collections.Generic;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;
using HollowKnight.Collision;

namespace HollowKnight.Player
{
    public class TheKnight : IPlayer
    {
        private readonly Dictionary<KnightSpriteType, ISprite> sprites;
        private ISprite currentSprite;
        private KnightSpriteType currentSpriteType;

        public Rectangle[] hitBoxes = new Rectangle[1];
        public Direction Facing { get; private set; } = Direction.Right;
        public Vector2 position;

        private readonly KnightPhysics physics = new();
        private readonly KnightCombat combat = new();
        private readonly KnightHealth health = new();
        private readonly KnightDash dash = new();        
        private int currentItem;

        private readonly Vector2 baseSize;

        public KnightState CurrentState { get; private set; } = KnightState.Idle;
        public KnightProjectile Projectiles { get; set; }

        public bool IsActive => true;
        public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, (int)baseSize.X, (int)baseSize.Y);
        public float VelocityY => physics.Velocity.Y;

        public TheKnight(Dictionary<KnightSpriteType, ISprite> sprites, Vector2 position)
        {
            this.sprites = sprites;
            this.position = position;

            currentSpriteType = KnightSpriteType.Idle;
            currentSprite = this.sprites[currentSpriteType];
            currentSprite.SetPosition(position);

            baseSize = this.sprites[KnightSpriteType.Idle].GetSize();
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update subsystems
            physics.Update(dt);
            if (combat.IsCastPulseActive)
                physics.StopMovingVertical();
            position += physics.Velocity * dt;

            // Absorb sub-pixel gravity drift when grounded to prevent vertical jitter
            if (physics.IsGrounded)
                position.Y = MathF.Floor(position.Y);

            health.Update(gameTime);
            combat.Update(gameTime, position, Facing, currentSprite);
            dash.Update(gameTime.ElapsedGameTime.TotalSeconds);

            if (health.IsHealing)
            {
                CurrentState = KnightState.Healing;
                KnightSpriteType? healState = health.UpdateHeal(gameTime);
                if (healState.HasValue)
                    currentSpriteType = healState.Value;
                physics.StopMovingHorizontal();
            }
            else if (combat.IsAttacking)
            {
                CurrentState = KnightState.Attacking;
                currentSpriteType = combat.AttackType;
            }
            else if (dash.IsDashing)
            {
                CurrentState = KnightState.Dashing;
                currentSpriteType = KnightSpriteType.Dashing;
                physics.ApplyDashVelocity(dash.GetDashDirection());
                physics.Velocity.Y = 0;
            }
            else if (health.IsDamaged)
            {
                CurrentState = KnightState.Damaged;
                currentSpriteType = KnightSpriteType.Damaged;
            }
            else if (combat.IsCastPulseActive)
            {
                CurrentState = KnightState.Spellcasting;
                currentSpriteType = KnightSpriteType.SpiritCast;
            }
            else if (!physics.IsGrounded)
            {
                CurrentState = physics.Velocity.Y > 0 ? KnightState.Falling : KnightState.Jumping;
                currentSpriteType = KnightSpriteType.Jumping;
            }
            else if (physics.Velocity.X != 0)
            {
                CurrentState = KnightState.Running;
                currentSpriteType = KnightSpriteType.Walking;
            }
            else
            {
                CurrentState = KnightState.Idle;
                currentSpriteType = KnightSpriteType.Idle;
            }

            // Stop movement when dash ends
            if (dash.DashEnded())
            {
                physics.StopMovingHorizontal();
            }

            currentSprite = sprites[currentSpriteType];
            
            // Align the visual sprite's bottom to match the fixed hitbox bottom, keeping feet on the ground
            Vector2 currentSize = currentSprite.GetSize();
            float spriteOffsetY = baseSize.Y - currentSize.Y;
            
            currentSprite.SetPosition(new Vector2(position.X, position.Y + spriteOffsetY));
            currentSprite.Update(gameTime);
        }

        public Rectangle[] GetBounds()
        {
            // Lock hitbox size to baseSize to prevent physics jitter during animation state changes
            hitBoxes[0] = new Rectangle(
                (int)position.X + CollisionConstants.KnightHitboxOffsetX, 
                (int)position.Y + CollisionConstants.KnightHitboxOffsetY, 
                (int)baseSize.X - CollisionConstants.KnightHitboxWidthShrink, 
                (int)baseSize.Y - CollisionConstants.KnightHitboxHeightShrink
            );
            return hitBoxes;
        }

        public Rectangle GetHurtbox()
        {
            Rectangle[] bounds = GetBounds();
            bounds[0].Inflate(-CollisionConstants.KnightHurtboxShrink, -CollisionConstants.KnightHurtboxShrink);
            return bounds[0];
        }

        public string GetStateName() => CurrentState.ToString();
        public double GetAttackCooldownRemaining() => combat.GetCooldownRemaining();
        public double GetInvincibilityCooldownRemaining() => health.GetInvincibilityCooldownRemaining();
        public double GetDashCooldownRemaining() => dash.GetDashCooldownRemaining();
        public double GetHealCooldownRemaining() => health.GetHealCooldownRemaining();
        public double GetCastCooldownRemaining() => combat.GetCastCooldownRemaining();
        public int Soul => health.Soul;
        public void GainSoul() => health.GainSoul();

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = Facing == Direction.Right
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;
            currentSprite.Draw(spriteBatch, effects);
            combat.DrawSlashEffect(spriteBatch, Facing);
            combat.DrawCastPulseEffect(spriteBatch, Facing);
        }

        // --- Movement ---
        public void MoveRight()
        {
            if (combat.IsAttacking || dash.IsDashing || combat.IsCastPulseActive) return;
            health.CancelHeal();
            Facing = Direction.Right;
            physics.MoveRight();
        }

        public void MoveLeft()
        {
            if (combat.IsAttacking || dash.IsDashing || combat.IsCastPulseActive) return;
            health.CancelHeal();
            Facing = Direction.Left;
            physics.MoveLeft();
        }

        public void MoveUp() => Console.WriteLine("Camera Move Up");
        public void MoveDown() => Console.WriteLine("Camera Move Down");

        public void Jump()
        {
            if (dash.IsDashing) return;
            if (!physics.IsGrounded) return;
            health.CancelHeal();
            if (physics.Jump())
            {
                dash.OnJump();
            }
        }

        public void StartDash()
        {
            health.CancelHeal();
            dash.StartDash(Facing, physics.IsGrounded);
        }

        public void StopJump()
        {
            physics.StopJump();
        }

        public void StopMovingHorizontal() => physics.StopMovingHorizontal();
        public void StopMovingVertical() => physics.StopMovingVertical();
        public void Land()
        {
            physics.Land();
            dash.OnLanded();
        }

        public void SetAirborne() => physics.SetAirborne();

        // --- Combat ---
        public void SideSlash()
        {
            if (dash.IsDashing) return;
            health.CancelHeal();
            combat.TryStartAttack(KnightSpriteType.SideSlash, position, physics.IsGrounded);
        }

        public void UpSlash()
        {
            if (dash.IsDashing) return;
            health.CancelHeal();
            combat.TryStartAttack(KnightSpriteType.UpSlash, position, physics.IsGrounded);
        }

        public void DownSlash()
        {
            if (dash.IsDashing) return;
            health.CancelHeal();
            combat.TryStartAttack(KnightSpriteType.DownSlash, position, physics.IsGrounded);
        }

        public SwordHitbox GetSwordHitbox() => combat.GetSwordHitbox(GetBounds()[0], Facing);

        // --- Health ---
        public void TakeDamage() => TakeDamage(CollisionSide.None);

        public void TakeDamage(CollisionSide side)
        {
            if (!health.TakeDamage()) return;
            Console.WriteLine("Knight took damage from " + side + " side");
            dash.CancelDash();
            physics.ApplyKnockback(side);
        }

        public int GetHealth() => health.Health;

        public void StartHeal()
        {
            if (dash.IsDashing) return;
            health.StartHeal(combat.IsAttacking, physics.IsGrounded);
        }
        public void CancelHeal() => health.CancelHeal();

        // --- Spells ---
        public void CastSpell()
        {
            if (health.Soul < 36) { Console.WriteLine("Not enough soul to cast spell"); return; }
            if (!combat.TryStartCastPulse(position)) return;
            health.ConsumeSoul(36);
            Console.WriteLine("Casting spell! Remaining soul: " + health.Soul);
            physics.ApplyCastKnockback(Facing);
            Projectiles?.Fire();
        }

        public void GiveFullSoul()
        {
            health.GiveFullSoul();
        }

        // --- Items ---
        public void UseItem(int itemNumber)
        {
            currentItem = itemNumber;
            Console.WriteLine($"Using item #{currentItem}");
        }

        public void Collect(CollisionSide side) => Console.WriteLine("Knight picked up a power up!");
        public void Block(CollisionSide side) => Console.WriteLine("Knight is colliding with a block");

        // --- Position ---
        public Vector2 GetPosition() => position;

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
            physics.Velocity = Vector2.Zero;
            currentSprite.SetPosition(position);
        }
    }
}
