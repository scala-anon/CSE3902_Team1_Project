using System;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Player
{
  public class KnightDash
  {

    public bool IsDashing {get; private set;}
    public bool DashAvailable {get; private set;}
    public bool IsOnDashCooldown {get; private set;}

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
        if(dashTimer >= GameConstants.KnightDashDuration)
        {
          IsDashing = false;
          dashJustEnded = true;
          dashTimer= 0;
        }
      }
      else if(IsOnDashCooldown)
      {
        dashCooldownTimer += dt;
        if(dashCooldownTimer >= GameConstants.KnightDashCooldown)
        {
          IsOnDashCooldown = false;
          dashCooldownTimer = 0;
        }
      }
    }

    public void StartDash(Direction direction, bool isGrounded)
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
    public float GetDashSpeed() => GameConstants.KnightDashSpeed;
    public bool DashEnded() => dashJustEnded;
    public double GetDashCooldownRemaining() => IsOnDashCooldown ? Math.Max(0, GameConstants.KnightDashCooldown - dashCooldownTimer) : 0;
  }
}