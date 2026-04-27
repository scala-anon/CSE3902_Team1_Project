using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using HollowKnight.Enemies;
using Microsoft.Xna.Framework;

namespace HollowKnight.Shared
{
    public static class EnemyConstants
    {
        // Shared
        public const float EnemyKnockbackSpeed = 950f;
        public const float EnemyKnockbackDecay = 8f;
        public const float EnemyKnockbackStopThreshold = 1f;
        public const float EnemyDeathGravity = 600f;
        public const double EnemyDamagedDuration = 0.4;
        public const int EnemyHurtboxGrow = 6;
        public const int EnemyDefaultHealth = 3;
        public const int EnemyDeadGroundSink = 50;

        // Crawlid
        public const float CrawlidPatrolSpeed = 175f;
        public const float CrawlidTurnDuration = 0.08f;
        public const int CrawlidDamage = 1;
        public const float CrawlidGravity = 600f;

        // Vengefly
        public const float VengeflyDetectionRadius = 400f;
        public const float VengeflyChaseRadius = 1200f;
        public const float VengeflyPatrolSpeed = 95f;
        public const float VengeflyChaseSpeed = 130f;
        public const double VengeflyStartleDuration = 0.5;
        public const float VengeflyPathUpdateInterval = 0.3f;
        public const float VengeflyWaypointReachDivisor = 1.5f;
        public const float VengeflyPathFailBackoffInitial = 0.6f;      // first backoff delay, seconds
        public const float VengeflyPathFailBackoffMax = 2.0f;          // backoff cap, seconds
        public const float VengeflyPathFailBackoffMultiplier = 2.0f;   // growth per failure
        public const int VengeflyPathFailuresBeforeBackoff = 2;        // consecutive failures needed to trigger backoff
        public const float VengeflyKnockbackUpComponent = -150f;
        public const float VengeflyVerticalKnockbackSpeed = 250f;
        public const int VengeflyDamage = 1;

        // Projectile
        public const float EnemyProjectileInterval = 3f;
        public const float EnemyProjectileSpeed = 250f;

        // Mantis Lord
        public const int MantisLordMiddleHealth          = 36;
        public const int MantisLordSideHealth            = 18;

        public const float MantisAttackCooldown          = 2.0f;
        public const float MantisSiblingStaggerDelay     = 0.6f;
        public const float MantisWallAttackInterval      = 6.0f;
        public const float MantisPostDeathToWoundedDelay = 0.2f;
        public const float MantisFrameInterval           = 0.1f;
        public const float MantisWallReadyDuration       = 1.0f;

        public const float MantisThroneLeftOffsetX       = -245f;
        public const float MantisThroneMiddleOffsetX     =2f;
        public const float MantisThroneRightOffsetX      = 255f;
        public const float MantisPrimaryThroneOffsetY    = -35f;
        public const float MantisSecondaryThroneOffsetY = 70f;

        public const float LeftMantisStandingX = 4274;
        public const float RightMantisStandingX = 4778;
        public const float MiddleMantisStandingX = 4524;  
        public const float SecondaryMantisStandingY = 4820;
        public const float PrimaryMantisStandingY = 4716;
        public const float MantisStabSpeed = 2840f;
        public const float MantisDashSpeed = 3750f;
        public const float MantisWallHangLeftX = 3625f;
        public const float MantisWallHangRightX = 5060f;//5115f;
        public const float MantisWallHangY = 4546f;
        public const float MantisDashArriveLeftX = 3750f;
        public const float MantisDashArriveRightX = 4800f;
        public const float MantisDashY = 5050f;
        public const float MantisDashArriveSpriteHeigthOffset = 556/2;
        public const float MantisDashAnticipateSpriteHeightOffset = 556/3;
        public const float MantisWallHangOffset = 199;

        public const float MantisProjectileXVelocity = 800f;
        public const float MantisProjectileXAcceleration = -400f;
        public const float MantisProjectileYSpeed = 100f;
        public const float MantisProjectileDuration = 20f;
        public const float MantisProjectile2XVelocity = 450f;
        public const float MantisProjectile2XAcceleration = -300f;
        public const float MantisProjectile2YSpeed = 100f;



        public const float Power2 = 2f;
        public const float half = .5f;
    
        //FIX
        
        public static readonly Dictionary<MantisLordState, Rectangle> mantisLordHitBoxes = new Dictionary<MantisLordState, Rectangle>()
        {
            [MantisLordState.IdleOnThrone] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.ThroneStand] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.ThroneLeave] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.ThroneArrive] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.ThroneWounded] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.ThroneBow] = new Rectangle(0, 0, 0, 0),

            // Requested categories
            [MantisLordState.Throw] = new Rectangle(0, 0, 614, 570),
            [MantisLordState.DashAnticipate] = new Rectangle(0, 0, 470, 284),
            [MantisLordState.Dash] = new Rectangle(0, 0, 557, 176),
            [MantisLordState.DashRecover] = new Rectangle(0, 0, 482, 243),
            [MantisLordState.DStabLand] = new Rectangle(0, 0, 321, 357),
            [MantisLordState.WallReady] = new Rectangle(0, 0, 250, 504),

            // Everything else zeroed
            [MantisLordState.DashArrive] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.DashLeave] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.DStabArrive] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.DStabOffset] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.DStab] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.DStabLandOffset] = new Rectangle(0,0,0,0),
            [MantisLordState.DStabLeave] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.WallArrive] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.WallLeave1] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.WallLeave2] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.Death] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.DeathLeaveOne] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.DeathLeaveTwo] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.Dormant] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.GracePeriod] = new Rectangle(0, 0, 0, 0),
            [MantisLordState.DStabStart] = new Rectangle(0,0,0,0),
            [MantisLordState.DashStart] = new Rectangle(0,0,0,0),
            [MantisLordState.WallStart] = new Rectangle(0,0,0,0),
            };
        }
}
