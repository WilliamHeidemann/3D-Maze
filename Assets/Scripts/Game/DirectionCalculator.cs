using System;
using Graph;
using UnityEngine;

namespace Game
{
    public class DirectionCalculator
    {
        private Orientation face;
        private CardinalDirection facing;

        public DirectionCalculator(Orientation startingFace)
        {
            face = startingFace;
            facing = CardinalDirection.North;
        }
        
        public CardinalDirection CalculateWorldDirection(CardinalDirection keyPressDirection)
        {
            // Debug.Log($"Facing: {facing}");
            return facing switch
            {
                CardinalDirection.North => keyPressDirection,
                CardinalDirection.South => keyPressDirection.Opposite(),
                CardinalDirection.East => keyPressDirection switch
                {
                    CardinalDirection.North => CardinalDirection.East,
                    CardinalDirection.South => CardinalDirection.West,
                    CardinalDirection.East => CardinalDirection.South,
                    CardinalDirection.West => CardinalDirection.North,
                    _ => throw new ArgumentOutOfRangeException(nameof(keyPressDirection), keyPressDirection, null)
                },
                CardinalDirection.West => keyPressDirection switch
                {
                    CardinalDirection.North => CardinalDirection.West,
                    CardinalDirection.South => CardinalDirection.East,
                    CardinalDirection.East => CardinalDirection.North,
                    CardinalDirection.West => CardinalDirection.South,
                    _ => throw new ArgumentOutOfRangeException(nameof(keyPressDirection), keyPressDirection, null)
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void HandleFaceChange(CardinalDirection worldDirection)
        {
            // Assumes the orientation face has just changed
            // Changes face and facing
            // Should be called when switching to a new orientation and moved past an edge in "worldDirection".
            switch (worldDirection)
            {
                case CardinalDirection.North:
                    switch (face)
                    {
                        case Orientation.Front:
                            face = Orientation.Up;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case Orientation.Back:
                            face = Orientation.Up;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.East;
                                    break;
                            }
                            break;
                        case Orientation.Right:
                            face = Orientation.Up;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.South;
                                    break;
                            }
                            break;
                        case Orientation.Left:
                            face = Orientation.Up;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.North;
                                    break;
                            }
                            break;
                        case Orientation.Up:
                            face = Orientation.Back;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.East;
                                    break;
                            }
                            break;
                        case Orientation.Down:
                            face = Orientation.Front;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case CardinalDirection.South: 
                    switch (face)
                    {
                        case Orientation.Front:
                            face = Orientation.Down;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Back:
                            face = Orientation.Down;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.East;
                                    break;
                            }
                            break;
                        case Orientation.Right:
                            face = Orientation.Down;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.North;
                                    break;
                            }
                            break;
                        case Orientation.Left:
                            face = Orientation.Down;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.South;
                                    break;
                            }
                            break;
                        case Orientation.Up:
                            face = Orientation.Front;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Down:
                            face = Orientation.Back;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.East;
                                    break;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case CardinalDirection.East: 
                    switch (face)
                    {
                        case Orientation.Front:
                            face = Orientation.Right;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Back:
                            face = Orientation.Left;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Right:
                            face = Orientation.Back;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Left:
                            face = Orientation.Front;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Up:
                            face = Orientation.Right;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.North;
                                    break;
                            }
                            break;
                        case Orientation.Down:
                            face = Orientation.Right;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.South;
                                    break;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case CardinalDirection.West: // 
                    switch (face)
                    {
                        case Orientation.Front:
                            face = Orientation.Left;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Back:
                            face = Orientation.Right;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Right: 
                            face = Orientation.Front;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Left:
                            face = Orientation.Back;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Up:
                            face = Orientation.Left;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.South;
                                    break;
                            }
                            break;
                        case Orientation.Down:
                            face = Orientation.Left;
                            switch (facing)
                            {
                                case CardinalDirection.North:
                                    facing = CardinalDirection.East;
                                    break;
                                case CardinalDirection.East:
                                    facing = CardinalDirection.South;
                                    break;
                                case CardinalDirection.South:
                                    facing = CardinalDirection.West;
                                    break;
                                case CardinalDirection.West:
                                    facing = CardinalDirection.North;
                                    break;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(worldDirection), worldDirection, null);
            }
        }
    }
}