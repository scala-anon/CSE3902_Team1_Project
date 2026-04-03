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

    private readonly float dashSpeed = GameConstants.KnightDashSpeed;
    private readonly double dashDuration = GameConstants.KnightDashDuration;
    private readonly double dashCooldown = GameConstants.KnightDashCooldown;


    public void Update(double dt)
    {
      dashJustEnded = false;
      if (IsDashing)
      {
        dashTimer += dt;
        if(dashTimer >= dashDuration)
        {
          IsDashing = false;
          dashJustEnded = true;
          dashTimer= 0;
        }
      }

      if(IsOnDashCooldown)
      {
        dashCooldownTimer += dt;
        if(dashCooldownTimer >= dashCooldown)
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
      // Grounded: allow dash with cooldown
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
      // Only reset dash availability if cooldown is finished and we're grounded
      if (!IsOnDashCooldown)
      {
        DashAvailable = true;
        isCurrentlyAirborne = true;
      }
    }

    public void OnLanded()
    {
      isCurrentlyAirborne = false;
      // Dash availability will be managed by cooldown on ground
    }

    public void CancelDash()
    {
      if (!IsDashing) return;
      IsDashing = false;
    }

    public Direction GetDashDirection() => dashDirection;
    public float GetDashSpeed() => dashSpeed;
    public bool DashEnded() => dashJustEnded;
  }
}