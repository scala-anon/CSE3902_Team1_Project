using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using HollowKnight.Factories;

namespace HollowKnight.Player
{
    public class KnightCombat
    {
        public KnightSpriteType AttackType { get; private set; } = KnightSpriteType.SideSlash;
        public bool IsAttacking { get; private set; }
        public bool IsAttackOnCooldown { get; private set; }

        private double attackTimer;
        private double attackCooldownTimer;

        private ISprite slashEffect;
        private bool isSlashEffectActive;

        private ISprite castPulseEffect;
        private bool isCastPulseActive;
        public bool IsCastPulseActive => isCastPulseActive;
        private double castPulseTimer;
        private readonly double castPulseDuration = KnightConstants.KnightCastPulseDuration;

        public bool IsCastOnCooldown { get; private set; }
        private double castCooldownTimer;
        private readonly double castCooldown = KnightConstants.KnightCastPulseDuration * 2;

        public void Update(GameTime gameTime, Vector2 position, Direction facing, ISprite currentSprite)
        {
            double dt = gameTime.ElapsedGameTime.TotalSeconds;
            UpdateAttackTimer(dt);
            UpdateAttackCooldown(dt);
            UpdateCastCooldown(dt);
            UpdateSlashEffect(gameTime, position, facing, currentSprite);
            UpdateCastPulse(gameTime, dt, position, facing, currentSprite);
        }

        private void UpdateAttackTimer(double dt)
        {
            if (!IsAttacking) return;
            attackTimer += dt;
            if (attackTimer >= KnightConstants.KnightAttackDuration)
            {
                IsAttacking = false;
                attackTimer = 0;
                isSlashEffectActive = false;
                slashEffect = null;
            }
        }

        private void UpdateAttackCooldown(double dt)
        {
            if (!IsAttackOnCooldown) return;
            attackCooldownTimer += dt;
            if (attackCooldownTimer >= KnightConstants.KnightAttackCooldown)
            {
                IsAttackOnCooldown = false;
                attackCooldownTimer = 0;
            }
        }

        private void UpdateCastCooldown(double dt)
        {
            if (!IsCastOnCooldown) return;
            castCooldownTimer += dt;
            if (castCooldownTimer >= castCooldown)
            {
                IsCastOnCooldown = false;
                castCooldownTimer = 0;
            }
        }

        private void UpdateSlashEffect(GameTime gameTime, Vector2 position, Direction facing, ISprite currentSprite)
        {
            if (!isSlashEffectActive || slashEffect == null) return;
            Vector2 knightSize = currentSprite.GetSize();
            Vector2 slashPosition = position;

            switch (AttackType)
            {
                case KnightSpriteType.SideSlash:
                    slashPosition.X += facing == Direction.Right
                        ? knightSize.X - knightSize.X / KnightConstants.SlashEffectRightDivisor
                        : -slashEffect.Width + knightSize.X / KnightConstants.SlashEffectLeftDivisor;
                    slashPosition.Y += knightSize.Y / KnightConstants.SlashEffectYDivisor;
                    break;
                case KnightSpriteType.UpSlash:
                    slashPosition.X += (knightSize.X - slashEffect.Width) / 2;
                    slashPosition.Y -= slashEffect.Height - knightSize.Y / KnightConstants.UpSlashEffectYDivisor;
                    break;
                case KnightSpriteType.DownSlash:
                    slashPosition.X += (knightSize.X - slashEffect.Width) / 2 - knightSize.X / KnightConstants.DownSlashEffectXDivisor;
                    slashPosition.Y += knightSize.Y - knightSize.Y / KnightConstants.DownSlashEffectYDivisor;
                    break;
            }
            slashEffect.SetPosition(slashPosition);
            slashEffect.Update(gameTime);
        }

        private void UpdateCastPulse(GameTime gameTime, double dt, Vector2 position, Direction facing, ISprite currentSprite)
        {
            if (!isCastPulseActive || castPulseEffect == null) return;
            castPulseTimer += dt;
            if (castPulseTimer >= castPulseDuration)
            {
                isCastPulseActive = false;
                castPulseEffect = null;
                return;
            }
            Vector2 knightSize = currentSprite.GetSize();
            Vector2 pulsePosition = position;
            pulsePosition.X += facing == Direction.Right
                ? 0
                : -castPulseEffect.Width + knightSize.X;
            pulsePosition.Y += (knightSize.Y - castPulseEffect.Height) / KnightConstants.CastPulseYDivisor;
            castPulseEffect.SetPosition(pulsePosition);
            castPulseEffect.Update(gameTime);
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

            return true;
        }

        public SwordHitbox GetSwordHitbox(Rectangle knightBounds, Direction facing)
        {
            if (!IsAttacking) return null;
            return new SwordHitbox(knightBounds, facing, AttackType);
        }

        public double GetCooldownRemaining() =>
            IsAttackOnCooldown ? System.Math.Max(0, KnightConstants.KnightAttackCooldown - attackCooldownTimer) : 0;

        public double GetCastCooldownRemaining() =>
            IsCastOnCooldown ? System.Math.Max(0, castCooldown - castCooldownTimer) : 0;

        public void DrawSlashEffect(SpriteBatch spriteBatch, Direction facing)
        {
            if (!isSlashEffectActive || slashEffect == null) return;
            SpriteEffects effects = facing == Direction.Right
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;
            slashEffect.Draw(spriteBatch, effects);
        }

        public bool TryStartCastPulse(Vector2 position)
        {
            if (IsCastOnCooldown) return false;
            isCastPulseActive = true;
            castPulseTimer = 0;
            castPulseEffect = SpriteFactory.Instance.CreateSpiritPulseSprite(position);
            IsCastOnCooldown = true;
            castCooldownTimer = 0;
            return true;
        }

        public void DrawCastPulseEffect(SpriteBatch spriteBatch, Direction facing)
        {
            if (!isCastPulseActive || castPulseEffect == null) return;
            SpriteEffects effects = facing == Direction.Right
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;
            castPulseEffect.Draw(spriteBatch, effects);
        }
    }
}
