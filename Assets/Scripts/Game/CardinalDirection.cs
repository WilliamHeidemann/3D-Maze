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

        public static CardinalDirection Clockwise90Degrees(this CardinalDirection direction)
        {
            return direction switch
            {
                CardinalDirection.North => CardinalDirection.East,
                CardinalDirection.South => CardinalDirection.West,
                CardinalDirection.East => CardinalDirection.South,
                CardinalDirection.West => CardinalDirection.North,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static CardinalDirection CounterClockwise90Degrees(this CardinalDirection direction)
        {
            return direction switch
            {
                CardinalDirection.North => CardinalDirection.West,
                CardinalDirection.South => CardinalDirection.East,
                CardinalDirection.East => CardinalDirection.North,
                CardinalDirection.West => CardinalDirection.South,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}