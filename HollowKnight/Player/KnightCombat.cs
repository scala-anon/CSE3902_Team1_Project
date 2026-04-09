using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Factories;
using HollowKnight.Audio;
using Microsoft.Xna.Framework.Audio;

namespace HollowKnight.Player
{
    public class KnightCombat
    {
        public KnightSpriteType AttackType { get; private set; } = KnightSpriteType.SideSlash;
        public bool IsAttacking { get; private set; }
        public bool IsAttackOnCooldown { get; private set; }

        private double attackTimer;
        private readonly double attackDuration = GameConstants.KnightAttackDuration;
        private readonly double attackCooldown = GameConstants.KnightAttackCooldown;
        private double attackCooldownTimer;

        private ISprite slashEffect;
        private bool isSlashEffectActive;

        public void Update(GameTime gameTime, Vector2 position, Direction facing, ISprite currentSprite)
        {
            double dt = gameTime.ElapsedGameTime.TotalSeconds;

            if (IsAttacking)
            {
                attackTimer += dt;
                if (attackTimer >= attackDuration)
                {
                    IsAttacking = false;
                    attackTimer = 0;
                    isSlashEffectActive = false;
                    slashEffect = null;
                }
            }

            if (IsAttackOnCooldown)
            {
                attackCooldownTimer += dt;
                if (attackCooldownTimer >= attackCooldown)
                {
                    IsAttackOnCooldown = false;
                    attackCooldownTimer = 0;
                }
            }

            if (isSlashEffectActive && slashEffect != null)
            {
                Vector2 slashPosition = position;
                Vector2 knightSize = currentSprite.GetSize();

                switch (AttackType)
                {
                    case KnightSpriteType.SideSlash:
                        slashPosition.X += facing == Direction.Right
                            ? knightSize.X - knightSize.X / 7
                            : -slashEffect.Width + knightSize.X / 5;
                        slashPosition.Y += knightSize.Y / 10;
                        break;
                    case KnightSpriteType.UpSlash:
                        slashPosition.X += (knightSize.X - slashEffect.Width) / 2;
                        slashPosition.Y -= slashEffect.Height - knightSize.Y / 4;
                        break;
                    case KnightSpriteType.DownSlash:
                        slashPosition.X += (knightSize.X - slashEffect.Width) / 2 - knightSize.X / 10;
                        slashPosition.Y += knightSize.Y - knightSize.Y / 3;
                        break;
                }
                slashEffect.SetPosition(slashPosition);
                slashEffect.Update(gameTime);
            }
        }

        public bool TryStartAttack(KnightSpriteType type, Vector2 position, bool isGrounded)
        {
            if (IsAttackOnCooldown) return false;
            if (type == KnightSpriteType.DownSlash && isGrounded) return false;

            AttackType = type;
            IsAttacking = true;
            attackTimer = 0;
            IsAttackOnCooldown = true;
            attackCooldownTimer = 0;

            isSlashEffectActive = true;
            slashEffect = type switch
            {
                KnightSpriteType.SideSlash => SpriteFactory.Instance.CreateSideSlashEffect(position),
                KnightSpriteType.UpSlash => SpriteFactory.Instance.CreateUpSlashEffect(position),
                KnightSpriteType.DownSlash => SpriteFactory.Instance.CreateDownSlashEffect(position),
                _ => null
            };

            SoundEffect Attack = AudioLoader.Instance.Get_Hero_Attack();
            AudioManager.Instance.PlaySoundEffect(Attack, GameConstants.HalfVolume, GameConstants.Pitch, GameConstants.Pan, false); 

            return true;
        }

        public SwordHitbox GetSwordHitbox(Rectangle knightBounds, Direction facing)
        {
            if (!IsAttacking) return null;
            return new SwordHitbox(knightBounds, facing, AttackType);
        }

        public double GetCooldownRemaining() =>
            IsAttackOnCooldown ? System.Math.Max(0, attackCooldown - attackCooldownTimer) : 0;

        public void DrawSlashEffect(SpriteBatch spriteBatch, Direction facing)
        {
            if (!isSlashEffectActive || slashEffect == null) return;
            SpriteEffects effects = facing == Direction.Right
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;
            slashEffect.Draw(spriteBatch, effects);
        }
    }
}
