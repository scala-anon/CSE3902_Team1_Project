
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace HollowKnight.Audio
{
    public class AudioLoader
    {

        private SoundEffect hero_jump;
        private SoundEffect hero_take_damage;
        private SoundEffect hero_dash;
        private SoundEffect hero_run;
        private SoundEffect grass_cut;
        private SoundEffect hero_attack;
        private SoundEffect hero_land;
        private static AudioLoader instance = new AudioLoader();

        public static AudioLoader Instance
        {
            get {return instance;}
        }
        
        public void loadAudio(ContentManager Content)
        {
            hero_jump = Content.Load<SoundEffect>(SoundId.PlayerJump);
            hero_take_damage = Content.Load<SoundEffect>(SoundId.PlayerDamage);
            hero_dash = Content.Load<SoundEffect>(SoundId.PlayerDash);
            hero_run = Content.Load<SoundEffect>(SoundId.PlayerRun);
            grass_cut = Content.Load<SoundEffect>(SoundId.GrassCut);
            hero_attack = Content.Load<SoundEffect>(SoundId.PlayerAttack);
            hero_land = Content.Load<SoundEffect>(SoundId.PlayerLand);
        }

        public SoundEffect Get_Hero_Attack()
        {
            return hero_attack;
        }

        public SoundEffect Get_Hero_Land()
        {
            return hero_land;
        }
        public SoundEffect Get_Hero_Jump()
        {
            return hero_jump;
        }

        public SoundEffect Get_Hero_Take_Damage()
        {
            return hero_take_damage;
        }

        public SoundEffect Get_Hero_Dash()
        {
            return hero_dash;
        }

        public SoundEffect Get_Hero_Run()
        {
            return hero_run;
        }

        public SoundEffect Get_Grass_Cut()
        {
            return grass_cut;
        }


    }
}