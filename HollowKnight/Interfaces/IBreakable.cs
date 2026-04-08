namespace HollowKnight.Interfaces
{
    public interface IBreakable
    {
        int Health { get; } // used for objects that take multiple hits
        void Break();
    }
}