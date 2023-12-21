using System;

namespace Game
{
    public enum CardinalDirection
    {
        North,
        South,
        East,
        West
    }

    public static class CardinalDirectionExtensionMethods
    {
        public static CardinalDirection Opposite(this CardinalDirection direction)
        {
            return direction switch
            {
                CardinalDirection.North => CardinalDirection.South,
                CardinalDirection.South => CardinalDirection.North,
                CardinalDirection.East => CardinalDirection.West,
                CardinalDirection.West => CardinalDirection.East,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}