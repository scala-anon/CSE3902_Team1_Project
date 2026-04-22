namespace HollowKnight.Shared
{
    [System.Flags]
    public enum CollisionLayer
    {
        None             = 0,
        Player           = 1 << 0,
        PlayerAttack     = 1 << 1,
        Enemy            = 1 << 2,
        Hazard           = 1 << 3,
        Terrain          = 1 << 4,
        Interactable     = 1 << 5,
        PlayerProjectile = 1 << 6,
        EnemyProjectile  = 1 << 7,
        Pickup           = 1 << 8,
    }
}
