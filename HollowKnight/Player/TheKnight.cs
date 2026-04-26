using System;
using System.Collections.Generic;
using HollowKnight.Factories;
using HollowKnight.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HollowKnight.Shared;
using HollowKnight.Collision;
using HollowKnight.Audio;
using HollowKnight.Storage;
using Microsoft.Xna.Framework.Audio;


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
        public static bool GodmodeEnabled = false;
        public KnightProjectile Projectiles { get; set; }
        public KnightDash Dash => dash;

        public bool IsActive => true;
        public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, (int)baseSize.X, (int)baseSize.Y);
        public float VelocityY => physics.VelocityY;

        private Vector2 benchSpawnPoint;
        private int _benchSpawnRoom = 1;
        private bool _justDied;
        private bool _needsBenchRoomTransition;

        public int BenchSpawnRoom => _benchSpawnRoom;
        public Vector2 BenchSpawnPoint => benchSpawnPoint;
        public void SetBenchRoom(int room) => _benchSpawnRoom = room;
        public bool NeedsBenchRoomTransition => _needsBenchRoomTransition;
        public void ConsumeBenchRoomTransition() => _needsBenchRoomTransition = false;

        private bool _isSitting;
        private double _sittingTimer;
        private ISprite _sittingSprite;
        private ISprite _sittingIdleSprite;
        private ISprite _sittingIdleLongSprite;
        private const double SittingIdleDelay = 0.3;
        private const double SittingIdleLongDelay = 5.0;
        private float _sittingStartX;
        private float _sittingTargetX;
        private float _sittingStartY;
        private float _sittingTargetY;
        private bool _isStandingUp;
        private double _standingUpTimer;
        private ISprite _standingUpSprite;
        public bool JustDied => _justDied;
        public void ConsumeJustDied() => _justDied = false;
        public void SetBenchSpawnPoint(Vector2 pos) => benchSpawnPoint = pos;
        public bool IsSitting => _isSitting;
        public void StartSitting(Vector2 benchPosition)
        {
            if (!physics.IsGrounded) return;
            float targetX = benchPosition.X + CollisionConstants.BenchHitboxWidth / 2f - baseSize.X / 2f - 6f;
            float targetY = position.Y + 5f;
            benchSpawnPoint = new Vector2(targetX, targetY);
            if (CurrentState != KnightState.Idle && CurrentState != KnightState.Running) return;
            _isSitting = true;
            _sittingTimer = 0;
            _sittingStartX = position.X;
            _sittingTargetX = targetX;
            _sittingStartY = position.Y;
            _sittingTargetY = targetY;
            physics.Velocity = Vector2.Zero;
        }
        public void StopSitting()
        {
            _isSitting = false;
            _sittingTimer = 0;
        }
        public void StartSittingIdle()
        {
            _isSitting = true;
            _isStandingUp = false;
            _sittingTimer = SittingIdleDelay;
            _sittingTargetX = benchSpawnPoint.X;
            _sittingTargetY = benchSpawnPoint.Y;
            position.X = _sittingTargetX;
            position.Y = _sittingTargetY;
            physics.Velocity = Vector2.Zero;
        }

        private Vector2? _roomRespawnPoint;
        public bool HasRespawnPoint => _roomRespawnPoint.HasValue;
        public void SetRoomRespawnPoint(Vector2 pos) => _roomRespawnPoint = pos;
        public void ClearRoomRespawnPoint() => _roomRespawnPoint = null;

        public void Respawn()
        {
            SetPosition(_roomRespawnPoint.Value);
            physics.Velocity = Vector2.Zero;
            health.CancelDamageState();
        }

        public TheKnight(Dictionary<KnightSpriteType, ISprite> sprites, Vector2 position)
        {
            this.sprites = sprites;
            this.position = position;
            benchSpawnPoint = position;

            currentSpriteType = KnightSpriteType.Idle;
            currentSprite = this.sprites[currentSpriteType];
            currentSprite.SetPosition(position);

            baseSize = this.sprites[KnightSpriteType.Idle].GetSize();

            _sittingSprite = this.sprites[KnightSpriteType.Sitting];
            _sittingIdleSprite = SpriteFactory.Instance.CreateKnightSittingIdleSprite(position);
            _sittingIdleLongSprite = SpriteFactory.Instance.CreateKnightSittingIdleLongSprite(position);
            _standingUpSprite = SpriteFactory.Instance.CreateKnightStandingUpSprite(position);
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

            if (_isSitting)
            {
                _sittingTimer += dt;
                CurrentState = KnightState.Sitting;
                currentSpriteType = KnightSpriteType.Sitting;

                if (_isStandingUp)
                {
                    _standingUpTimer += dt;
                    if (_standingUpTimer >= SittingIdleDelay)
                    {
                        _isSitting = false;
                        _isStandingUp = false;
                        _sittingTimer = 0;
                        _standingUpTimer = 0;
                        _standingUpSprite = SpriteFactory.Instance.CreateKnightStandingUpSprite(position);
                        return;
                    }
                    currentSprite = _standingUpSprite;
                    currentSprite.SetPosition(new Vector2(position.X, position.Y - 5f));
                    currentSprite.Update(gameTime);
                    return;
                }

                if (_sittingTimer >= SittingIdleDelay)
                {
                    position.X = _sittingTargetX;
                    position.Y = _sittingTargetY;
                    currentSprite = _sittingTimer >= SittingIdleLongDelay ? _sittingIdleLongSprite : _sittingIdleSprite;
                    currentSprite.SetPosition(new Vector2(position.X, position.Y - 18f));
                }
                else
                {
                    float t = (float)(_sittingTimer / SittingIdleDelay);
                    float tEased = 1f - (1f - t) * (1f - t); //equation for easing out when sittingn on bench
                    position.X = _sittingStartX + (_sittingTargetX - _sittingStartX) * tEased;
                    position.Y = _sittingStartY + (_sittingTargetY - _sittingStartY) * tEased;
                    currentSprite = _sittingSprite;
                    currentSprite.SetPosition(new Vector2(position.X, position.Y - 5f));
                }

                currentSprite.Update(gameTime);
                return;
            }

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
                physics.ZeroVerticalVelocity();
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
                CurrentState = physics.IsFalling ? KnightState.Falling : KnightState.Jumping;
                currentSpriteType = KnightSpriteType.Jumping;
            }
            else if (physics.IsMovingHorizontally)
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

        public void Draw(SpriteBatch spriteBatch, float layerDepth = 0f)
        {
            SpriteEffects effects = Facing == Direction.Right
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally;
            currentSprite.Draw(spriteBatch, effects, layerDepth);
            combat.DrawSlashEffect(spriteBatch, Facing, GameConstants.LayerDepthKnightEffects);
            combat.DrawCastPulseEffect(spriteBatch, Facing, GameConstants.LayerDepthKnightEffects);
        }

        // --- Movement ---
        public void MoveRight()
        {
            if (_isSitting) { if (!_isStandingUp && _sittingTimer >= SittingIdleDelay) { _isStandingUp = true; _standingUpTimer = 0; } return; }
            if (combat.IsAttacking || dash.IsDashing || combat.IsCastPulseActive) return;
            health.CancelHeal();
            Facing = Direction.Right;
            physics.MoveRight();
        }

        public void MoveLeft()
        {
            if (_isSitting) { if (!_isStandingUp && _sittingTimer >= SittingIdleDelay) { _isStandingUp = true; _standingUpTimer = 0; } return; }
            if (combat.IsAttacking || dash.IsDashing || combat.IsCastPulseActive) return;
            health.CancelHeal();
            Facing = Direction.Left;
            physics.MoveLeft();
        }

        public void MoveUp() { }
        public void MoveDown() { }

        public void Jump()
        {
            if (_isSitting) { if (!_isStandingUp && _sittingTimer >= SittingIdleDelay) { _isStandingUp = true; _standingUpTimer = 0; } return; }
            if (dash.IsDashing) return;
            if (!physics.IsGrounded) return;
            health.CancelHeal();
            if (physics.Jump())
            {
                AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Hero_Jump());
                dash.OnJump();
            }
        }

        public void StartDash()
        {
            health.CancelHeal();
            AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Hero_Dash());
            dash.StartDash(Facing, physics.IsGrounded, position);
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
            if (GodmodeEnabled)
            {
                DebugLogger.LogGeneral($"Godmode: ignored damage from {side} side");
                return;
            }
            if (!health.TakeDamage()) return;
            AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Hero_Take_Damage());
            DebugLogger.LogGeneral($"Knight took damage from {side} side, health={health.Health}");
            dash.CancelDash();

            if (health.Health == 0)
            {
                _justDied = true;
                DebugLogger.LogGeneral($"Knight died. Respawning at {benchSpawnPoint} (room {_benchSpawnRoom}).");
                health.ResetHealth();
                _needsBenchRoomTransition = true;
            }
            else
            {
                physics.ApplyKnockback(side);
            }
        }

        public int GetHealth() => health.Health;
        public int GetMaxHealth() => health.MaxHealth;
        public void FullHeal() => health.ResetHealth();
        public int GetSoul() => health.Soul;
        public int GetMaxSoul() => health.MaxSoul;
        public float GetSoulFillRatio() => health.GetSoulFillRatio();
        public bool IsDead() => health.Health <= 0;
        public void GainSoul(int amount) => health.AddSoul(amount);

        public void StartHeal()
        {
            if (dash.IsDashing) return;
            health.StartHeal(combat.IsAttacking, physics.IsGrounded);
        }
        public void CancelHeal() => health.CancelHeal();

        // --- Spells ---
        public void CastSpell()
        {
            if (health.Soul < KnightConstants.KnightSpellCastSoulCost) { DebugLogger.LogGeneral("Not enough soul to cast spell"); return; }
            if (!combat.TryStartCastPulse(position)) return;
            sprites[KnightSpriteType.SpiritCast].Reset();
            health.ConsumeSoul(KnightConstants.KnightSpellCastSoulCost);
            DebugLogger.LogGeneral($"Casting spell! Remaining soul: {health.Soul}");
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
            string selected = ItemManager.CurrentItem;
            DebugLogger.LogGeneral($"Using item #{currentItem} ({selected})");

            if (selected == ItemManager.VengefulSpiritItemName)
            {
                CastSpell();
                return;
            }

            // Boomerang / Bomb: stubs — future work
        }

        public void Collect(CollisionSide side)
        {
            health.AddSoul(GameConstants.SpiritPickupSoul);
            DebugLogger.LogGeneral($"Knight collected soul. Soul is now {health.Soul}/{health.MaxSoul}");
        }
        public void Block(CollisionSide side) => DebugLogger.LogCollision($"Knight is colliding with a block on side={side}");

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
