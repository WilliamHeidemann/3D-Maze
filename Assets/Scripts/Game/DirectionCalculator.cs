using System;
using Graph;
using UnityEngine;

namespace Game
{
    public class DirectionCalculator
    {
        public Orientation nearestOrientation;
        public CardinalDirection nearestDirection;

        private Orientation targetOrientation;
        private CardinalDirection targetDirection;

        public DirectionCalculator(Orientation startingNearestOrientation)
        {
            nearestOrientation = startingNearestOrientation;
            nearestDirection = CardinalDirection.North;
            targetOrientation = startingNearestOrientation;
            targetDirection = CardinalDirection.North;
        }
        
        public CardinalDirection CalculateWorldDirection(CardinalDirection keyPressDirection)
        {
            return nearestDirection switch
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
        
        public Quaternion GetRotation()
        {
            switch (targetDirection)
            {
                case CardinalDirection.North:
                    switch (targetOrientation)
                    {
                        case Orientation.Front:
                            return Quaternion.Euler(0f, 0f, 0f);
                        case Orientation.Back:
                            return Quaternion.Euler(0f, 180f, 0f);
                        case Orientation.Right:
                            return Quaternion.Euler(0f, 90f, 0f);
                        case Orientation.Left:
                            return Quaternion.Euler(0f, 270f, 0f);
                        case Orientation.Up:
                            return Quaternion.Euler(270f, 0f, 0f);
                        case Orientation.Down:
                            return Quaternion.Euler(90f, 0f, 0f);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case CardinalDirection.South:
                    switch (targetOrientation)
                    {
                        case Orientation.Front:
                            return Quaternion.Euler(0f, 0f, 180f);
                        case Orientation.Back:
                            return Quaternion.Euler(0f, 180f, 180f);
                        case Orientation.Right:
                            return Quaternion.Euler(0f, 270f, 180f);
                        case Orientation.Left:
                            return Quaternion.Euler(0f, 90f, 180f);
                        case Orientation.Up:
                            return Quaternion.Euler(90f, 180f, 0f);
                        case Orientation.Down:
                            return Quaternion.Euler(270f, 180f, 0f);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case CardinalDirection.East:
                    switch (targetOrientation)
                    {
                        case Orientation.Front:
                            return Quaternion.Euler(0f, 0f, 90f);
                        case Orientation.Back:
                            return Quaternion.Euler(0f, 180f, 270f);
                        case Orientation.Right:
                            return Quaternion.Euler(270f, 90f, 0f);
                        case Orientation.Left:
                            return Quaternion.Euler(90f, 270f, 0f);
                        case Orientation.Up:
                            return Quaternion.Euler(0f, 270f, 90f);
                        case Orientation.Down:
                            return Quaternion.Euler(0f, 90f, 90f);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case CardinalDirection.West:
                    switch (targetOrientation)
                    {
                        case Orientation.Front:
                            return Quaternion.Euler(0f, 0f, 270f);
                        case Orientation.Back:
                            return Quaternion.Euler(0f, 180f, 90f);
                        case Orientation.Right:
                            return Quaternion.Euler(90f, 90f, 0f);
                        case Orientation.Left:
                            return Quaternion.Euler(270f, 270f, 0f);
                        case Orientation.Up:
                            return Quaternion.Euler(0f, 90f, 270f);
                        case Orientation.Down:
                            return Quaternion.Euler(0f, 270f, 270f);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void SetTargetRotationDirection(Orientation targetOrientationParameter)
        {
            targetOrientation = targetOrientationParameter;
            targetDirection = CalculateTargetRotationDirection();
        }
        
        private CardinalDirection CalculateTargetRotationDirection()
        {
            if (nearestOrientation == targetOrientation) return nearestDirection;
            if (nearestOrientation.Opposite() == targetOrientation)
            {
                Debug.Log("Went directly to the opposite site.");
                return nearestDirection; 
                // Not always correct. Fails on super low frame rate
                // when the player might travel to the opposite site of the cube.
            }
            
            switch (nearestOrientation)
            {
                case Orientation.Front:
                    return nearestDirection;
                case Orientation.Back:
                    switch (targetOrientation)
                    {
                        case Orientation.Right:
                        case Orientation.Left:
                            return nearestDirection;
                        case Orientation.Up:
                            return nearestDirection.Opposite();
                        case Orientation.Down:
                            return nearestDirection.Opposite();
                        default:
                            throw new ArgumentOutOfRangeException();
                    };
                case Orientation.Right:
                    switch (targetOrientation)
                    {
                        case Orientation.Front:
                        case Orientation.Back:
                            return nearestDirection;
                        case Orientation.Up:
                            return nearestDirection.CounterClockwise90Degrees();
                        case Orientation.Down:
                            return nearestDirection.Clockwise90Degrees();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case Orientation.Left:
                    switch (targetOrientation)
                    {
                        case Orientation.Front:
                        case Orientation.Back:
                            return nearestDirection;
                        case Orientation.Up:
                            return nearestDirection.Clockwise90Degrees();
                        case Orientation.Down:
                            return nearestDirection.CounterClockwise90Degrees();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case Orientation.Up:
                    switch (targetOrientation)
                    {
                        case Orientation.Front:
                            return nearestDirection;
                        case Orientation.Back:
                            return nearestDirection.Opposite();
                        case Orientation.Right:
                            return nearestDirection.Clockwise90Degrees();
                        case Orientation.Left:
                            return nearestDirection.CounterClockwise90Degrees();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case Orientation.Down:
                    switch (targetOrientation)
                    {
                        case Orientation.Front:
                            return nearestDirection;
                        case Orientation.Back:
                            return nearestDirection.Opposite();
                        case Orientation.Right:
                            return nearestDirection.CounterClockwise90Degrees();
                        case Orientation.Left:
                            return nearestDirection.Clockwise90Degrees();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void HandleLogicalFaceChange(CardinalDirection worldDirection)
        {
            // Assumes the orientation face has just changed
            // Changes face and facing
            // Should be called when switching to a new orientation and moved past an edge in "worldDirection".
            switch (worldDirection)
            {
                case CardinalDirection.North:
                    switch (nearestOrientation)
                    {
                        case Orientation.Front:
                            nearestOrientation = Orientation.Up;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            break;
                        case Orientation.Back:
                            nearestOrientation = Orientation.Up;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                            }
                            break;
                        case Orientation.Right:
                            nearestOrientation = Orientation.Up;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                            }
                            break;
                        case Orientation.Left:
                            nearestOrientation = Orientation.Up;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                            }
                            break;
                        case Orientation.Up:
                            nearestOrientation = Orientation.Back;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                            }
                            break;
                        case Orientation.Down:
                            nearestOrientation = Orientation.Front;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case CardinalDirection.South: 
                    switch (nearestOrientation)
                    {
                        case Orientation.Front:
                            nearestOrientation = Orientation.Down;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Back:
                            nearestOrientation = Orientation.Down;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                            }
                            break;
                        case Orientation.Right:
                            nearestOrientation = Orientation.Down;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                            }
                            break;
                        case Orientation.Left:
                            nearestOrientation = Orientation.Down;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                            }
                            break;
                        case Orientation.Up:
                            nearestOrientation = Orientation.Front;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Down:
                            nearestOrientation = Orientation.Back;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case CardinalDirection.East: 
                    switch (nearestOrientation)
                    {
                        case Orientation.Front:
                            nearestOrientation = Orientation.Right;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Back:
                            nearestOrientation = Orientation.Left;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Right:
                            nearestOrientation = Orientation.Back;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Left:
                            nearestOrientation = Orientation.Front;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Up:
                            nearestOrientation = Orientation.Right;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                            }
                            break;
                        case Orientation.Down:
                            nearestOrientation = Orientation.Right;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case CardinalDirection.West: 
                    switch (nearestOrientation)
                    {
                        case Orientation.Front:
                            nearestOrientation = Orientation.Left;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Back:
                            nearestOrientation = Orientation.Right;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Right: 
                            nearestOrientation = Orientation.Front;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Left:
                            nearestOrientation = Orientation.Back;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                            }
                            break;
                        case Orientation.Up:
                            nearestOrientation = Orientation.Left;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.North;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                            }
                            break;
                        case Orientation.Down:
                            nearestOrientation = Orientation.Left;
                            switch (nearestDirection)
                            {
                                case CardinalDirection.North:
                                    nearestDirection = CardinalDirection.East;
                                    break;
                                case CardinalDirection.East:
                                    nearestDirection = CardinalDirection.South;
                                    break;
                                case CardinalDirection.South:
                                    nearestDirection = CardinalDirection.West;
                                    break;
                                case CardinalDirection.West:
                                    nearestDirection = CardinalDirection.North;
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