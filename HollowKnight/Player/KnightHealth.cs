using System;
using Microsoft.Xna.Framework;
using HollowKnight.Shared;

namespace HollowKnight.Player
{
    public class KnightHealth
    {
        public int Health { get; private set; } = GameConstants.KnightStartHealth;
        public int MaxHealth { get; } = GameConstants.KnightMaxHealth;
        public bool IsDamaged { get; private set; }
        public bool IsHealing { get; private set; }

        private double damagedTimer;
        private readonly double invincibilityDuration = GameConstants.KnightInvincibilityDuration;

        private bool healApplied;
        private double healTimer;
        private readonly double healPrepDuration = GameConstants.KnightHealPrepDuration;
        private readonly double healPostDuration = GameConstants.KnightHealPostDuration;

        public void Update(GameTime gameTime)
        {
            if (IsDamaged)
            {
                damagedTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (damagedTimer >= invincibilityDuration)
                {
                    IsDamaged = false;
                    damagedTimer = 0;
                }

                
            }
        }

        public bool TakeDamage()
        {
            if (IsDamaged) return false;
            IsDamaged = true;
            damagedTimer = 0;
            Health = Math.Max(0, Health - 1);
            CancelHeal();
            return true;
        }

        public void StartHeal(bool isAttacking, bool isGrounded)
        {
            if (isAttacking || !isGrounded) return;
            if (IsHealing) return;

            IsHealing = true;
            healApplied = false;
            healTimer = 0;
        }

        public void CancelHeal()
        {
            if (!IsHealing) return;
            IsHealing = false;
            healApplied = false;
            healTimer = 0;
        }

        /// <summary>
        /// Returns the current heal animation state, or null if not healing.
        /// </summary>
        public KnightSpriteType? UpdateHeal(GameTime gameTime)
        {
            if (!IsHealing) return null;

            healTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (healTimer < healPrepDuration)
                return KnightSpriteType.HealPrep;

            if (!healApplied)
            {
                healApplied = true;
                if (Health < MaxHealth)
                {
                    Health++;
                    Console.WriteLine($"Healed! Health is now {Health}");
                }
                else
                {
                    Console.WriteLine("Heal finished, but already at max health");
                }
            }

            if (healTimer < healPrepDuration + healPostDuration)
                return KnightSpriteType.HealPost;

            IsHealing = false;
            healApplied = false;
            healTimer = 0;
            return null;
        }

        public double GetInvincibilityCooldownRemaining() =>
            IsDamaged ? Math.Max(0, invincibilityDuration - damagedTimer) : 0;
    }
}
