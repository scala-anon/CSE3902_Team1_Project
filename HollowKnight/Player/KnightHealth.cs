using System;
using Microsoft.Xna.Framework;
using HollowKnight.Shared;

namespace HollowKnight.Player
{
    public class KnightHealth
    {
        public int Health { get; private set; } = GameConstants.KnightStartHealth;
        public int MaxHealth { get; } = GameConstants.KnightMaxHealth;
        
        public int Soul { get; private set; } = GameConstants.KnightStartSoul;
        public int MaxSoul { get; } = GameConstants.KnightMaxSoul;

        public bool IsDamaged { get; private set; }
        public bool IsHealing { get; private set; }

        private double damagedTimer;

        private enum HealPhase { None, Startup, Prep, Post }
        private HealPhase healPhase = HealPhase.None;
        private double healTimer;

        public void GainSoul()
        {
            Soul = Math.Min(MaxSoul, Soul + GameConstants.KnightSoulPerHit);
        }

        public void ConsumeSoul(int amount)
        {
            Soul = Math.Max(0, Soul - amount);
        }

        public void GiveFullSoul()
        {
            Soul = MaxSoul;
        }

        public void Update(GameTime gameTime)
        {
            if (IsDamaged)
            {
                damagedTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (damagedTimer >= GameConstants.KnightInvincibilityDuration)
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

        public void ResetHealth()
        {
            Health = MaxHealth;
            IsDamaged = false;
            CancelHeal();
        }

        public void StartHeal(bool isAttacking, bool isGrounded)
        {
            if (isAttacking || !isGrounded) return;
            if (IsHealing) return;
            if (Soul < GameConstants.KnightSoulPerHeal) { Console.WriteLine("Not enough soul!"); return; }

            IsHealing = true;
            healPhase = HealPhase.Startup;
            healTimer = 0;
        }

        public void CancelHeal()
        {
            if (!IsHealing) return;
            IsHealing = false;
            healPhase = HealPhase.None;
            healTimer = 0;
        }

        /// <summary>
        /// Returns the current heal animation state, or null if not healing.
        /// </summary>
        public KnightSpriteType? UpdateHeal(GameTime gameTime)
        {
            if (!IsHealing) return null;
            healTimer += gameTime.ElapsedGameTime.TotalSeconds;
            return healPhase switch
            {
                HealPhase.Startup => HandleStartup(),
                HealPhase.Prep    => HandlePrep(),
                HealPhase.Post    => HandlePost(),
                _                => null
            };
        }

        private KnightSpriteType HandleStartup()
        {
            if (healTimer >= GameConstants.KnightHealStartUp)
            {
                healPhase = HealPhase.Prep;
                healTimer = 0;
            }
            return KnightSpriteType.HealPrep;
        }

        private KnightSpriteType HandlePrep()
        {
            if (healTimer >= GameConstants.KnightHealPrepDuration)
            {
                Soul -= GameConstants.KnightSoulPerHeal;
                Health = Math.Min(MaxHealth, Health + 1);
                Console.WriteLine($"Healed! Health is now {Health}");
                healPhase = HealPhase.Post;
                healTimer = 0;
            }
            return KnightSpriteType.HealPrep;
        }

        private KnightSpriteType HandlePost()
        {
            if (healTimer >= GameConstants.KnightHealPostDuration)
            {
                if (Soul >= GameConstants.KnightSoulPerHeal)
                {
                    healPhase = HealPhase.Prep;
                    healTimer = 0;
                }
                else
                {
                    CancelHeal();
                }
            }
            return KnightSpriteType.HealPost;
        }

        public double GetInvincibilityCooldownRemaining() =>
            IsDamaged ? Math.Max(0, GameConstants.KnightInvincibilityDuration - damagedTimer) : 0;

        public double GetHealCooldownRemaining() =>
            healPhase == HealPhase.Post ? Math.Max(0, GameConstants.KnightHealPostDuration - healTimer) : 0;
    }
}
