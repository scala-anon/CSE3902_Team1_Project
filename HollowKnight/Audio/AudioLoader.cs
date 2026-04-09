
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace HollowKnight.Audio
{
    public class AudioLoader
    {

        private readonly Dictionary<string, SoundEffect>sfx;
        private Song mantisLords;

        private AudioLoader()
        {
            sfx = new Dictionary<string, SoundEffect>();

        }
        private static AudioLoader instance = new AudioLoader();

        public static AudioLoader Instance
        {
            get {return instance;}
        }

        
        
        public void loadAudio(ContentManager Content)
        {
            XDocument AudioDoc = XDocument.Load("Content/Audio/AudioFiles.xml");
            XElement root = AudioDoc.Root;
            

            foreach (XElement item in root.Descendants("Item"))
            {
                var file_path = (string)item.Element("FilePath");
                SoundEffect sound = Content.Load<SoundEffect>(file_path);
                
                sfx.Add(file_path, sound);
                
            }
            

            #region Music
            mantisLords = Content.Load<Song>(SoundId.MantisLordsMusic);
            #endregion Music
        }
        
        #region Get Music
        public Song Get_Mantis_Lords_Music()
        {
            return mantisLords;
        }

        #endregion Get Music

        #region Get SFX

        public SoundEffect Get_Crawler_Walk()
        {
            return sfx[SoundId.CrawlidWalk];
        }

        public SoundEffect Get_Enemy_Damage()
        {
            return sfx[SoundId.EnemyDamage];
        }
        public SoundEffect Get_Hero_Attack()
        {
            
            return sfx[SoundId.PlayerAttack];
        }

        public SoundEffect Get_Hero_Land()
        {
            return sfx[SoundId.PlayerLand];
        }
        public SoundEffect Get_Hero_Jump()
        {
            return sfx[SoundId.PlayerJump];
        }

        public SoundEffect Get_Hero_Take_Damage()
        {
            return sfx[SoundId.PlayerDamage];
        }

        public SoundEffect Get_Hero_Dash()
        {
            return sfx[SoundId.PlayerDash];
        }

        public SoundEffect Get_Hero_Run()
        {
            return sfx[SoundId.PlayerRun];
        }

        public SoundEffect Get_Grass_Cut()
        {
            return sfx[SoundId.GrassCut];
        }
        #endregion Get SFX

    }
}