using System;
using HollowKnight.Projectiles;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Player
{
  public class KnightDash
  {

    public bool IsDashing {get; private set;}
    public bool DashAvailable {get; private set;}
    public bool IsOnDashCooldown {get; private set;}

    private ProjectileManager _projectileManager;
    public void SetProjectileManager(ProjectileManager pm) => _projectileManager = pm;

    private Direction dashDirection;
    private double dashTimer;
    private double dashCooldownTimer;
    private bool dashJustEnded;
    private bool isCurrentlyAirborne;



    public void Update(double dt)
    {
      dashJustEnded = false;
      if (IsDashing)
      {
        dashTimer += dt;
        if(dashTimer >= KnightConstants.KnightDashDuration)
        {
          IsDashing = false;
          dashJustEnded = true;
          dashTimer= 0;
        }
      }
      else if(IsOnDashCooldown)
      {
        dashCooldownTimer += dt;
        if(dashCooldownTimer >= KnightConstants.KnightDashCooldown)
        {
          IsOnDashCooldown = false;
          dashCooldownTimer = 0;
        }
      }
    }

    public void StartDash(Direction direction, bool isGrounded, Vector2 spawnPosition = default)
    {
      if (IsDashing) return;

      // Airborne: only allow dash if available (one per jump)
      if (!isGrounded)
      {
        if (!DashAvailable) return;
        IsDashing = true;
        DashAvailable = false;
        isCurrentlyAirborne = true;
      }
      // Grounded: dash cooldown
      else
      {
        if (IsOnDashCooldown) return;
        IsDashing = true;
        IsOnDashCooldown = true;
        dashCooldownTimer = 0;
        isCurrentlyAirborne = false;
      }

      dashDirection = direction;
      dashTimer = 0;
      _projectileManager?.Spawn(new DashEffect(spawnPosition, direction));
    }

    public void OnJump()
    {
      // reset dash if cooldown is finished and grounded
      if (!IsOnDashCooldown)
      {
        DashAvailable = true;
        isCurrentlyAirborne = true;
      }
    }

    public void OnLanded()
    {
      isCurrentlyAirborne = false;
    }

    public void CancelDash()
    {
      if (!IsDashing) return;
      IsDashing = false;
    }

    public Direction GetDashDirection() => dashDirection;
    public float GetDashSpeed() => KnightConstants.KnightDashSpeed;
    public bool DashEnded() => dashJustEnded;
    public double GetDashCooldownRemaining() => IsOnDashCooldown ? Math.Max(0, KnightConstants.KnightDashCooldown - dashCooldownTimer) : 0;
  }
}