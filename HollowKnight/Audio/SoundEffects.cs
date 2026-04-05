using System.ComponentModel;

namespace HollowKnight.Audio
{
    // TODO: Define all sound effect and music identifiers for the game.
    // Use this enum to request sounds from AudioManager so there are no magic strings.
    public static class SoundId 
    {
        // Player
        // PlayerAttack,
        public const string PlayerJump = "hero_jump.wav";
       
        public const string PlayerDamage = "hero_dash.wav";
      
        public const string PlayerDash = "her_dash.wav";
       
        public const string PlayerRun = "hero_run_foosteps_stone.wav";
        // PlayerHeal,
        // PlayerDeath,

      
        public const string GrassCut = "Grass Cut 1.mp3";


        // Enemies
        // CrawlidDeath,
        // VengeflyChase,
        // VengeflyDeath,

        // Environment
        // SpikeHit,

        // Music
        // BackgroundMusic,
        // BossMusic,
    }
}
