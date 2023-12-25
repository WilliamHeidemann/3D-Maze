using System;

namespace Graph
{
    public enum Orientation
    {
        Front,
        Back,
        Right,
        Left,
        Up,
        Down
    }
    
    public static class OrientationExtensionMethods
    {
        public static Orientation Opposite(this Orientation orientation)
        {
            return orientation switch
            {
                Orientation.Front => Orientation.Back,
                Orientation.Back => Orientation.Front,
                Orientation.Right => Orientation.Left,
                Orientation.Left => Orientation.Right,
                Orientation.Up => Orientation.Down,
                Orientation.Down => Orientation.Up,
                _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };
        }
    }
}