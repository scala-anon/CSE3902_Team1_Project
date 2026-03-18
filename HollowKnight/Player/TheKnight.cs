using System;
using System.Collections.Generic;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;
using HollowKnight.Collision;

namespace HollowKnight.Player
{
    public class TheKnight : IHollowKnight
    {
        private Dictionary<KnightSpriteType, ISprite> sprites;
        private ISprite currentSprite;
        private KnightSpriteType currentState;

        public Direction Facing { get; private set; } = Direction.Right;
        public Vector2 position;
        private Vector2 velocity;

        private float moveSpeed = 200f;
        private float jumpSpeed = -700f;
        private float gravity = 900f;
        private bool isGrounded;

        private bool isDamaged;
        private double damagedTimer;
        private double invincibilityDuration = 1.3; //same as actual game base stats
        private float knockbackSpeed = 250f;
        private float knockbackUpwards = -300f; //negative for upwards

        private int currentItem;

        private KnightSpriteType attackType;
        private bool isAttacking;
        private double attackTimer;
        private double attackDuration = 0.25;
        private double attackCooldown = 0.41;
        private double attackCooldownTimer = 0;
        private bool isAttackOnCooldown = false;

        private bool isHealing;
        private bool healApplied;
        private double healTimer;

        private double healPrepDuration = 0.6;
        private double healPostDuration = 0.2;

        private int health = 5;
        private int maxHealth = 9;

        public bool IsActive => true;
        public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, currentSprite.Width, currentSprite.Height);
        public float VelocityY => velocity.Y;

        public TheKnight(Dictionary<KnightSpriteType, ISprite> sprites, Vector2 position)
        {
            this.sprites = sprites;
            this.position = position;

            velocity = Vector2.Zero;
            isGrounded = false;

            attackType = KnightSpriteType.SideSlash;
            currentState = KnightSpriteType.Idle;
            currentSprite = this.sprites[currentState];
            currentSprite.SetPosition(position);
        }
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            velocity.Y += gravity * dt;
            position += velocity * dt;

            // Checks to see if Knight has been damaged
            if (isDamaged)
            {
                damagedTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (damagedTimer >= invincibilityDuration)
                {
                    isDamaged = false;
                    damagedTimer = 0;
                }
            }

            // Checks to see is knight is attacking
            if (isAttacking)
            {
                attackTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (attackTimer >= attackDuration)
                {
                    isAttacking = false;
                    attackTimer = 0;
                }
            }

            if (isAttackOnCooldown)
            {
                attackCooldownTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (attackCooldownTimer >= attackCooldown)
                {
                    isAttackOnCooldown = false;
                    attackCooldownTimer = 0;
                }
            }

            // Checks to see if knight is healing then changes currentSprite based on the action/movement of the knight
            if (isHealing)
            {
                UpdateHeal(gameTime);
            }
            else
            {
                if (isAttacking)
                {
                    currentState = attackType;
                }
                else if (isDamaged)
                {
                    currentState = KnightSpriteType.Damaged;
                }
                else if (!isGrounded)
                {
                    currentState = KnightSpriteType.Jumping;
                }
                else if (velocity.X != 0)
                {
                    currentState = KnightSpriteType.Walking;
                }
                else
                {
                    currentState = KnightSpriteType.Idle;
                }
            }
            currentSprite = sprites[currentState];
            currentSprite.SetPosition(position);
            currentSprite.Update(gameTime);
        }
        // TODO: Tune width/height to match the actual scaled sprite size
        public Rectangle GetBounds()
        {
            Vector2 size = currentSprite.GetSize();
            return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public string GetStateName() => currentState.ToString();
        
        public double GetAttackCooldownRemaining() => isAttackOnCooldown ? Math.Max(0, attackCooldown - attackCooldownTimer) : 0;

        public double GetInvincibilityCooldownRemaining() => isDamaged ? Math.Max(0, invincibilityDuration - damagedTimer) : 0;

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteEffects effects = (Facing == Direction.Right)
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;
            currentSprite.Draw(spriteBatch, effects);
        }
        public void MoveRight()
        {
            CancelHeal();
            Facing = Direction.Right;
            velocity.X = moveSpeed;
        }
        public void MoveLeft()
        {
            CancelHeal();
            Facing = Direction.Left;
            velocity.X = -moveSpeed;
        }
        public void MoveUp()
        {
            Console.WriteLine("Camera Move Up");
        }
        public void MoveDown()
        {
            Console.WriteLine("Camera Move Down");
        }
        public void TakeDamage()
        {
            TakeDamage(CollisionSide.None);
        }

        public SwordHitbox GetSwordHitbox()
        {
            if (!isAttacking) return null;
            return new SwordHitbox(GetBounds(), Facing, attackType);
        }

        public void TakeDamage(CollisionSide side)
        {
            if (isDamaged) return;
            Console.WriteLine("Knight took damage from " + side + " side");
            CancelHeal();
            isDamaged = true;
            damagedTimer = 0;
            health = Math.Max(0, health - 1);

            switch (side)
            {
                case CollisionSide.Left:
                    velocity.X = -knockbackSpeed;
                    velocity.Y = knockbackUpwards;
                    break;
                case CollisionSide.Right:
                    velocity.X = knockbackSpeed;
                    velocity.Y = knockbackUpwards;
                    break;
                case CollisionSide.Top:
                    velocity.Y = knockbackUpwards;
                    break;
                case CollisionSide.Bottom:
                    velocity.Y = -knockbackUpwards;
                    break;
            }
        }
        public void UseItem(int _itemNumber)
        {
            currentItem = _itemNumber;
            Console.WriteLine($"Using item #{currentItem}");
        }
        public void Jump()
        {
            CancelHeal();
            if (isGrounded)
            {
                velocity.Y = jumpSpeed;
                isGrounded = false;
            }
        }
        public void StopMovingHorizontal()
        {
            velocity.X = 0;
        }

        public void StopMovingVertical()
        {
            velocity.Y = 0;
        }
        public void Land()
        {
            velocity.Y = 0;
            isGrounded = true;
        }
        public void SideSlash()
        {
            if (!CanAttack()) return;
            CancelHeal();
            attackType = KnightSpriteType.SideSlash;
            StartAttack();
        }
        public void UpSlash()
        {
            if (!CanAttack()) return;
            CancelHeal();
            attackType = KnightSpriteType.UpSlash;
            StartAttack();
        }
        public void DownSlash()
        {
            if (!CanAttack()) return;
            CancelHeal();
            attackType = KnightSpriteType.DownSlash;
            StartAttack();
        }
        private bool CanAttack() => !isAttackOnCooldown;
        private void StartAttack()
        {
            isAttacking = true;
            attackTimer = 0;
            isAttackOnCooldown = true;
            attackCooldownTimer = 0;
        }
        public void StartHeal()
        {
            if (isAttacking || !isGrounded)
                return;

            if (!isHealing)
            {
                isHealing = true;
                healApplied = false;
                healTimer = 0;
                velocity.X = 0;
            }
        }
        public void CancelHeal()
        {
            if (!isHealing)
                return;

            isHealing = false;
            healApplied = false;
            healTimer = 0;
        }
        public void UpdateHeal(GameTime gameTime)
        {
            if (!isHealing)
                return;

            velocity.X = 0;

            healTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (healTimer < healPrepDuration)
            {
                currentState = KnightSpriteType.HealPrep;
                return;
            }

            if (!healApplied)
            {
                healApplied = true;

                if (health < maxHealth)
                {
                    health++;
                    Console.WriteLine($"Healed! Health is now {health}");
                }
                else
                {
                    Console.WriteLine("Heal finished, but already at max health");
                }
            }

            if (healTimer < healPrepDuration + healPostDuration)
            {
                currentState = KnightSpriteType.HealPost;
                return;
            }

            isHealing = false;
            healApplied = false;
            healTimer = 0;
        }
    }
}