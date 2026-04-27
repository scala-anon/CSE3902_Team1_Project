using System;
using Microsoft.Xna.Framework;
using HollowKnight.Shared;
using HollowKnight.Audio;

namespace HollowKnight.Player
{
    public class KnightHealth
    {
        public int Health { get; private set; } = KnightConstants.KnightStartHealth;
        public int MaxHealth { get; } = KnightConstants.KnightMaxHealth;
        
        public int Soul { get; private set; } = KnightConstants.KnightStartSoul;
        public int MaxSoul { get; } = KnightConstants.KnightMaxSoul;

        private bool AudioPlayed = false;
        public bool IsDamaged { get; private set; }
        public bool IsHealing { get; private set; }

        private double damagedTimer;

        private enum HealPhase { None, Startup, Prep, Post }
        private HealPhase healPhase = HealPhase.None;
        private double healTimer;

        public void GainSoul()
        {
            Soul = Math.Min(MaxSoul, Soul + KnightConstants.KnightSoulPerHit);
        }

        public void AddSoul(int amount)
        {
            Soul = Math.Clamp(Soul + amount, 0, MaxSoul);
        }

        public float GetSoulFillRatio() => MaxSoul <= 0 ? 0f : Soul / (float)MaxSoul;

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
                AudioPlayed = false;
                damagedTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (damagedTimer >= KnightConstants.KnightInvincibilityDuration)
                {
                    IsDamaged = false;
                    damagedTimer = 0;
                }
            }

            if (Health == 0)
            {
                if (AudioPlayed == false)
                {
                    AudioPlayed = true;
                    AudioManager.Instance.TryPlayGoofy(AudioLoader.Instance.Get_Goofy_Death());
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
            AudioManager.Instance.TryPlayGoofy(AudioLoader.Instance.Get_Goofy_Take_Damage());
            return true;
        }

        public void CancelDamageState()
        {
            IsDamaged = false;
            damagedTimer = 0;
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
            if (Soul < KnightConstants.KnightSoulPerHeal) { DebugLogger.LogGeneral("Not enough soul to heal"); return; }

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
            if (healTimer >= KnightConstants.KnightHealStartUp)
            {
                healPhase = HealPhase.Prep;
                healTimer = 0;
            }
            return KnightSpriteType.HealPrep;
        }

        private KnightSpriteType HandlePrep()
        {
            if (healTimer >= KnightConstants.KnightHealPrepDuration)
            {
                Soul -= KnightConstants.KnightSoulPerHeal;
                Health = Math.Min(MaxHealth, Health + 1);
                DebugLogger.LogGeneral($"Healed! Health is now {Health}/{MaxHealth}");
                healPhase = HealPhase.Post;
                healTimer = 0;
            }
            return KnightSpriteType.HealPrep;
        }

        private KnightSpriteType HandlePost()
        {
            if (healTimer >= KnightConstants.KnightHealPostDuration)
            {
                if (Soul >= KnightConstants.KnightSoulPerHeal)
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
            IsDamaged ? Math.Max(0, KnightConstants.KnightInvincibilityDuration - damagedTimer) : 0;

        public double GetHealCooldownRemaining() =>
            healPhase == HealPhase.Post ? Math.Max(0, KnightConstants.KnightHealPostDuration - healTimer) : 0;
    }
}
