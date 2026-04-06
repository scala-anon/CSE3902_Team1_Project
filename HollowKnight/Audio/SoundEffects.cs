using System.ComponentModel;

namespace HollowKnight.Audio
{
    // TODO: Define all sound effect and music identifiers for the game.
    // Use this enum to request sounds from AudioManager so there are no magic strings.
    public static class SoundId 
    {
        // Player
        public const string PlayerAttack = "Audio/hero_unsheath";
        public const string PlayerJump = "Audio/hero_jump";
       public const string PlayerLand = "Audio/hero_land_soft";
        public const string PlayerDamage = "Audio/hero_dash";
      
        public const string PlayerDash = "Audio/hero_dash";
       
        public const string PlayerRun = "Audio/hero_run_footsteps_stone";
        // PlayerHeal,
        // PlayerDeath,

      
        public const string GrassCut = "Audio/Grass Cut 1";


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
