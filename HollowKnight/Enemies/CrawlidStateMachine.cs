using System.Collections.Generic;
using HollowKnight.Audio;
using HollowKnight.Environment;
using HollowKnight.Interfaces;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Enemies
{
    public class CrawlidStateMachine
    {
        private int frameCounter = 0;
        private Crawlid CurrentCrawlid;
        private Direction _movementDirection = Direction.Right;
        private bool _isTurning = false;
        private float _turnTimer = 0f;
        private float _velocityY = 0f;
        private bool found = false;
        private List<IObject> _platforms = new List<IObject>();

        public CrawlidStateMachine(Crawlid enemy)
        {
            CurrentCrawlid = enemy;
        }

        public void SetPlatforms(List<IObject> platforms) => _platforms = platforms;

        public void ResetVerticalVelocity() => _velocityY = 0f;

        public void OnWallHit()
        {
            if (_isTurning) return;
            FlipDirection();
            CrawlidTurn();
        }

        public void ChangeHealth()
        {
            CurrentCrawlid.Health--;
            AudioManager.Instance.PlaySoundEffectIfInView(AudioLoader.Instance.Get_Enemy_Damage(), CurrentCrawlid.position);
            AudioManager.Instance.TryPlayGoofy(AudioLoader.Instance.Get_Goofy_Damage());
            if (CurrentCrawlid.Health <= 0)
            {
                AudioManager.Instance.TryPlayGoofy(AudioLoader.Instance.Get_Goofy_On_Kill());
                CurrentCrawlid.Alive = false;
                CurrentCrawlid.SetState(CurrentCrawlid.IsGrounded
                    ? CrawlidState.DeathLand
                    : CrawlidState.DeathAir);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!CurrentCrawlid.Alive || CurrentCrawlid.IsDamaged) return;
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!CurrentCrawlid.IsGrounded)
            {
                _velocityY += EnemyConstants.CrawlidGravity * dt;
                CurrentCrawlid.position.Y += _velocityY * dt;
                CurrentCrawlid.UpdateSpritePosition();
                return;
            }

            found = AudioManager.Instance.TryPlaySoundEffect(CurrentCrawlid.position);
            if (found == true)
            {
                AudioManager.Instance.TryPlayGoofy(AudioLoader.Instance.Get_Goofy_Crawlid());
                found = false;
            }

            _velocityY = 0f;

            if (frameCounter % 180 == 0)
                AudioManager.Instance.PlaySoundEffectIfInView(AudioLoader.Instance.Get_Crawler_Walk(), CurrentCrawlid.position);
            frameCounter++;

            if (_isTurning)
            {
                _turnTimer += dt;
                if (_turnTimer >= EnemyConstants.CrawlidTurnDuration)
                {
                    _isTurning = false;
                    _turnTimer = 0f;
                    CurrentCrawlid.SetState(CrawlidState.Idle);
                }
                CurrentCrawlid.UpdateSpritePosition();
                return;
            }

            float speed = _movementDirection == Direction.Right
                ? EnemyConstants.CrawlidPatrolSpeed
                : -EnemyConstants.CrawlidPatrolSpeed;
            CurrentCrawlid.position.X += speed * dt;

            float spriteWidth = CurrentCrawlid.SpriteSize.X;
            int dirSign = _movementDirection == Direction.Right ? 1 : -1;
            bool shouldTurn = false;

            // Detect spikes directly ahead — turn before reaching them
            Rectangle spikeProbe = new Rectangle(
                dirSign == 1
                    ? (int)(CurrentCrawlid.position.X + spriteWidth)
                    : (int)(CurrentCrawlid.position.X - CollisionConstants.CrawlidSpikeDetectRange),
                (int)CurrentCrawlid.position.Y,
                CollisionConstants.CrawlidSpikeDetectRange,
                (int)CurrentCrawlid.SpriteSize.Y);

            foreach (IObject obj in _platforms)
            {
                if (!(obj is Spike) || !obj.IsActive) continue;
                foreach (Rectangle spikeBox in obj.GetBounds())
                {
                    if (spikeProbe.Intersects(spikeBox))
                    {
                        shouldTurn = true;
                        break;
                    }
                }
                if (shouldTurn) break;
            }

            // Detect platform edge ahead — turn before walking off
            if (!shouldTurn && _platforms.Count > 0)
            {
                Rectangle feet = CurrentCrawlid.FeetRect;
                Rectangle edgeProbe = new Rectangle(
                    feet.X + dirSign * CollisionConstants.CrawlidEdgeProbeOffset,
                    feet.Y + CollisionConstants.CrawlidGroundProbeExtension,
                    feet.Width,
                    feet.Height + CollisionConstants.CrawlidGroundProbeExtension);

                bool groundAhead = false;
                foreach (IObject obj in _platforms)
                {
                    if (obj is Spike || !obj.IsActive) continue;
                    Rectangle pb = obj.Bounds;
                    if (edgeProbe.Intersects(pb) && edgeProbe.Left >= pb.Left && edgeProbe.Right <= pb.Right)
                    {
                        groundAhead = true;
                        break;
                    }
                }
                if (!groundAhead) shouldTurn = true;
            }

            if (shouldTurn)
            {
                FlipDirection();
                CrawlidTurn();
            }
            else
            {
                // Level-boundary fallback
                if (CurrentCrawlid.position.X + spriteWidth >= GameConstants.DefaultLevelWidth)
                {
                    CurrentCrawlid.position.X = GameConstants.DefaultLevelWidth - spriteWidth;
                    _movementDirection = Direction.Left;
                    CurrentCrawlid.FacingDirection = Direction.Left;
                    CrawlidTurn();
                }
                else if (CurrentCrawlid.position.X <= 0)
                {
                    CurrentCrawlid.position.X = 0;
                    _movementDirection = Direction.Right;
                    CurrentCrawlid.FacingDirection = Direction.Right;
                    CrawlidTurn();
                }
            }

            CurrentCrawlid.UpdateSpritePosition();
        }

        private void FlipDirection()
        {
            if (_movementDirection == Direction.Right)
            {
                _movementDirection = Direction.Left;
                CurrentCrawlid.FacingDirection = Direction.Left;
            }
            else
            {
                _movementDirection = Direction.Right;
                CurrentCrawlid.FacingDirection = Direction.Right;
            }
        }

        private void CrawlidTurn()
        {
            _isTurning = true;
            _turnTimer = 0f;
            CurrentCrawlid.SetState(CrawlidState.Turn);
        }

        public string GetStateName() => CurrentCrawlid.State.ToString();
    }
}
