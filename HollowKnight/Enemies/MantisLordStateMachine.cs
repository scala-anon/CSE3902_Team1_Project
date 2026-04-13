namespace HollowKnight.Enemies
{
    public class MantisLordStateMachine
    {
        //PRE-FIGHT stages
        //"Throne Idle"
        //Throne Idle ->  Throne Stand (ALL)
        //Throne stand -> Throne Leave (Middle)
        
        //FIGHT-STAGE Phase 1 (Only one lord)
        //210 HP 
        //Wall attack
            //Wall arrive
            //Wall Arrive -> Wall Ready
            //Wall Ready -> Throw
                //Throw -> Air Projectile
            //Throw -> Wall Leave Pt1
            //Wall Leave Pt1 -> Wall Leave Pt2
        //

        //Dash Attack 
            //Dash Arrive 
            //Dash arrive -> Dash Anticiapte
            //Dash Anticipate -> Dash
            //Dash -> Dash Recover
            //Dash Recover -> Dash Leave
        //

        //DStab
            //Dstab arrive
            //DStab Arrive -> DStab
            //DStab -> DStab Land
            //DStab Land -> DStab Leave
        //

        //On Death
            //Trigger Death
            //Death -> Death Leave 1
            //Death Leave 1 -> Throne Wounded (Middle Mantis)
           
        //

         //Throne Stand -> Throne Leave (Left and Right Mantis)

        //Phase 2 (Two Lords)
        //160 HP each
            // if wall attack both must trigger disk at same time 
                //Either high or low projectile throw
            // Can both dash at the same time if they are dashing in opposite directions
            // if death trigger leave 1 then throne wounded 

        //final phase
            // throne wounded -> throne standing -> throne bow


        //States
            //ThroneIdle
            //TroneStand
            //ThroneLeave
            //WallArrive
            //WallReady
            //Throw
            //Air Projectile
                //High
                //Low
            //WallLeave1
            //WallLeave2
            //DashArrive
            //DashAnticipate
            //DashRecover
            //DashLeave
            //DStabArrive
            //DStab
            //DStabLeave
            //Death
            //DeathLeave1
            //ThroneWounded
            //ThroneBow
        //
    }
}