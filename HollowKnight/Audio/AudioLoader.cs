
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace HollowKnight.Audio
{
    public class AudioLoader
    {
        public ContentManager _content;

        private AudioLoader(ContentManager Content)
        {
            _content = Content;
        }
        private void loadAudio()
        {
            SoundEffect hero_jump = _content.Load<SoundEffect>(SoundId.PlayerJump);
            SoundEffect hero_Damage = _content.Load<SoundEffect>(SoundId.PlayerDamage);
            SoundEffect hero_dash = _content.Load<SoundEffect>(SoundId.PlayerDash);
            SoundEffect hero_run = _content.Load<SoundEffect>(SoundId.PlayerRun);
            SoundEffect grass_cut = _content.Load<SoundEffect>(SoundId.GrassCut);
        }
    }
}