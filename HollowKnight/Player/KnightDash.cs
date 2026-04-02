using System;
using HollowKnight.Shared;
using Microsoft.Xna.Framework;

namespace HollowKnight.Player
{
  public class KnightDash
  {

    public bool IsDashing {get; private set;}
    public bool IsOnDashCooldown {get; private set;}

    private Direction dashDirection;
    private double dashTimer;
    private double dashCooldownTimer;
    private bool dashJustEnded;

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
        dashCooldownTimer +=dt;
        if(dashCooldownTimer>= dashCooldown)
        {
          IsOnDashCooldown = false;
          dashCooldownTimer = 0;
        }
      }
    }

    public void StartDash(Direction direction)
    {
      if (IsDashing || IsOnDashCooldown) return;

      IsDashing = true;
      IsOnDashCooldown = true;
      dashDirection = direction;
      dashTimer = 0;
      dashCooldownTimer = 0;
    }

    public void CancelDash()
    {
      if (!IsDashing) return;
      IsDashing = false;
      // dashTimer = 0;
    }

    public Direction GetDashDirection() => dashDirection;
    public float GetDashSpeed() => dashSpeed;
    public bool DashEnded() => dashJustEnded;
  }
}