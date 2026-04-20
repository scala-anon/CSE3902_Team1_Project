using Microsoft.Xna.Framework;

public class TransitionZone
{
    public Rectangle Bounds { get; }
    public int DestinationRoom { get; }

    public TransitionZone(Rectangle bounds, int destinationRoom)
    {
        Bounds = bounds;
        DestinationRoom = destinationRoom;
    }
}