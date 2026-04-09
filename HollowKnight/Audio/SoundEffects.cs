using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HollowKnight.Audio
{
    // TODO: Define all sound effect and music identifiers for the game.
    // Use this enum to request sounds from AudioManager so there are no magic strings.
    public static class SoundId 
    {
        // Player
        public const string PlayerAttack = "Audio/sfx/hero_unsheath";
        public const string PlayerJump = "Audio/sfx/hero_jump";
        public const string PlayerLand = "Audio/sfx/hero_land_soft";
        public const string PlayerDamage = "Audio/sfx/hero_dash";
        public const string PlayerDash = "Audio/sfx/hero_dash";
        public const string PlayerRun = "Audio/sfx/hero_run_footsteps_stone";
        // PlayerHeal,
        // PlayerDeath,
        public const string GrassCut = "Audio/sfx/Grass Cut 1";

        public const string EnemyDamage = "Audio/sfx/Enemy Damage";
        public const string CrawlidWalk = "Audio/sfx/Crawler";

        public const string MantisLordsMusic = "Audio/Music/Hollow Knight OST - Mantis Lords 4";


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
