namespace HollowKnight.Enemies
{
    public class MantisLordStateMachine
    {
        //PRE-FIGHT stages
        //"Throne Idle"
        //Throne Idle ->  Throne Stand (ALL)
        //Throne stand -> Throne Leave (Middle)
        
        //FIGHT-STAGE Phase 1 (Only one lord)
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
            // if wall attack both must trigger disk at same time 
            // if death trigger leave 1 then throne wounded 

        //final phase
            // throne wounded to throne standing to throne bow
    }
}