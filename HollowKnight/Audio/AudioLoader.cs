
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace HollowKnight.Audio
{
    public class AudioLoader
    {

        private readonly Dictionary<string, SoundEffect>sfx;
        private readonly Dictionary<string, Song>songs;
        private AudioLoader()
        {
            sfx = new Dictionary<string, SoundEffect>();
            songs = new Dictionary<string, Song>();

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

            XDocument SongDoc = XDocument.Load("Content/Audio/SongFiles.xml");
            XElement songRoot = SongDoc.Root;

            foreach (XElement item in songRoot.Descendants("Item"))
            {
                var file_path = (string)item.Element("FilePath");
                Song song = Content.Load<Song>(file_path);

                songs.Add(file_path, song);
                
            }
            
        }
        
        #region Get Music
        public Song Get_Mantis_Lords_Music()
        {
            return songs[SoundId.MantisLordsMusic];
        }

        public Song Get_Enter_Hollownest()
        {
            return songs[SoundId.EnterHollownest];
        }

        #endregion Get Music

        #region Get SFX

        public SoundEffect Get_Vengefly_Fly()
        {
            return sfx[SoundId.VengeFlyFly];
        }

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

        public SoundEffect Get_Hero_Fireball()
        {
            return sfx[SoundId.PlayerFireball];
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