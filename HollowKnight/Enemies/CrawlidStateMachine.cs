using System;
using HollowKnight.Audio;
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
        private IObject _platform;
        private bool in_camera;

        public void SetPlatform(IObject platform) => _platform = platform;

        public CrawlidStateMachine(Crawlid enemy)
        {
            CurrentCrawlid = enemy;
        }

        public void ChangeHealth()
        {
            CurrentCrawlid.Health--;
            AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Enemy_Damage());
            if (CurrentCrawlid.Health <= 0)
            {
                CurrentCrawlid.Alive = false;
                CurrentCrawlid.SetState(CurrentCrawlid.IsGrounded
                    ? CrawlidState.DeathLand
                    : CrawlidState.DeathAir);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!CurrentCrawlid.Alive || CurrentCrawlid.IsDamaged) return;
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            
            if (AudioManager.Instance.TryPlaySoundEffect(CurrentCrawlid.position) == true)
            {
                if (frameCounter % 180 == 0)
                {
                    AudioManager.Instance.PlaySoundEffect(AudioLoader.Instance.Get_Crawler_Walk());
                }
            }
            

            frameCounter++;
            if (_isTurning)
            {
                _turnTimer += elapsedTime;
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
            CurrentCrawlid.position.X += speed * elapsedTime;

            float spriteWidth = CurrentCrawlid.SpriteSize.X;
            float minX = 0;
            float maxX = GameConstants.DefaultLevelWidth;

            if (_platform != null)
            {
                Rectangle pBounds = _platform.Bounds;
                minX = pBounds.Left;
                maxX = pBounds.Right;
                CurrentCrawlid.position.Y = pBounds.Top - CurrentCrawlid.SpriteSize.Y;
            }

            if (CurrentCrawlid.position.X + spriteWidth >= maxX)
            {
                CurrentCrawlid.position.X = maxX - spriteWidth;
                _movementDirection = Direction.Left;
                CurrentCrawlid.FacingDirection = Direction.Left;
                CrawlidTurn();
            }
            else if (CurrentCrawlid.position.X <= minX)
            {
                CurrentCrawlid.position.X = minX;
                _movementDirection = Direction.Right;
                CurrentCrawlid.FacingDirection = Direction.Right;
                CrawlidTurn();
            }

            CurrentCrawlid.UpdateSpritePosition();
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
