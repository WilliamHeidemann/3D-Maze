using System;
using Graph;
using UnityEngine;

namespace Game
{
    public class DirectionCalculator
    {
        private Orientation nearestOrientation;
        private CardinalDirection nearestDirection;

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
                CardinalDirection.East => keyPressDirection.Clockwise90Degrees(),
                CardinalDirection.West => keyPressDirection.CounterClockwise90Degrees(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public Quaternion GetRotation()
        {
            return targetDirection switch
            {
                CardinalDirection.North => targetOrientation switch
                {
                    Orientation.Front => Quaternion.Euler(0f, 0f, 0f),
                    Orientation.Back => Quaternion.Euler(0f, 180f, 0f),
                    Orientation.Right => Quaternion.Euler(0f, 90f, 0f),
                    Orientation.Left => Quaternion.Euler(0f, 270f, 0f),
                    Orientation.Up => Quaternion.Euler(270f, 0f, 0f),
                    Orientation.Down => Quaternion.Euler(90f, 0f, 0f),
                    _ => throw new ArgumentOutOfRangeException()
                },
                CardinalDirection.South => targetOrientation switch
                {
                    Orientation.Front => Quaternion.Euler(0f, 0f, 180f),
                    Orientation.Back => Quaternion.Euler(0f, 180f, 180f),
                    Orientation.Right => Quaternion.Euler(0f, 270f, 180f),
                    Orientation.Left => Quaternion.Euler(0f, 90f, 180f),
                    Orientation.Up => Quaternion.Euler(90f, 180f, 0f),
                    Orientation.Down => Quaternion.Euler(270f, 180f, 0f),
                    _ => throw new ArgumentOutOfRangeException()
                },
                CardinalDirection.East => targetOrientation switch
                {
                    Orientation.Front => Quaternion.Euler(0f, 0f, 90f),
                    Orientation.Back => Quaternion.Euler(0f, 180f, 270f),
                    Orientation.Right => Quaternion.Euler(270f, 90f, 0f),
                    Orientation.Left => Quaternion.Euler(90f, 270f, 0f),
                    Orientation.Up => Quaternion.Euler(0f, 270f, 90f),
                    Orientation.Down => Quaternion.Euler(0f, 90f, 90f),
                    _ => throw new ArgumentOutOfRangeException()
                },
                CardinalDirection.West => targetOrientation switch
                {
                    Orientation.Front => Quaternion.Euler(0f, 0f, 270f),
                    Orientation.Back => Quaternion.Euler(0f, 180f, 90f),
                    Orientation.Right => Quaternion.Euler(90f, 90f, 0f),
                    Orientation.Left => Quaternion.Euler(270f, 270f, 0f),
                    Orientation.Up => Quaternion.Euler(0f, 90f, 270f),
                    Orientation.Down => Quaternion.Euler(0f, 270f, 270f),
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public void SetTargetRotationDirection(Orientation targetOrientationParameter)
        {
            targetOrientation = targetOrientationParameter;
            targetDirection = CalculateTargetRotationDirection();
        }
        
        private CardinalDirection CalculateTargetRotationDirection()
        {
            if (nearestOrientation == targetOrientation) return nearestDirection;
            if (nearestOrientation.Opposite() == targetOrientation) return nearestDirection; 
            
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
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => throw new ArgumentOutOfRangeException()
                            };
                            break;
                        case Orientation.Back:
                            nearestOrientation = Orientation.Up;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.South,
                                CardinalDirection.East => CardinalDirection.West,
                                CardinalDirection.South => CardinalDirection.North,
                                CardinalDirection.West => CardinalDirection.East,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Right:
                            nearestOrientation = Orientation.Up;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.West,
                                CardinalDirection.East => CardinalDirection.North,
                                CardinalDirection.South => CardinalDirection.East,
                                CardinalDirection.West => CardinalDirection.South,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Left:
                            nearestOrientation = Orientation.Up;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.East,
                                CardinalDirection.East => CardinalDirection.South,
                                CardinalDirection.South => CardinalDirection.West,
                                CardinalDirection.West => CardinalDirection.North,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Up:
                            nearestOrientation = Orientation.Back;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.South,
                                CardinalDirection.East => CardinalDirection.West,
                                CardinalDirection.South => CardinalDirection.North,
                                CardinalDirection.West => CardinalDirection.East,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Down:
                            nearestOrientation = Orientation.Front;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
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
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Back:
                            nearestOrientation = Orientation.Down;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.South,
                                CardinalDirection.East => CardinalDirection.West,
                                CardinalDirection.South => CardinalDirection.North,
                                CardinalDirection.West => CardinalDirection.East,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Right:
                            nearestOrientation = Orientation.Down;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.East,
                                CardinalDirection.East => CardinalDirection.South,
                                CardinalDirection.South => CardinalDirection.West,
                                CardinalDirection.West => CardinalDirection.North,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Left:
                            nearestOrientation = Orientation.Down;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.West,
                                CardinalDirection.East => CardinalDirection.North,
                                CardinalDirection.South => CardinalDirection.East,
                                CardinalDirection.West => CardinalDirection.South,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Up:
                            nearestOrientation = Orientation.Front;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Down:
                            nearestOrientation = Orientation.Back;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.South,
                                CardinalDirection.East => CardinalDirection.West,
                                CardinalDirection.South => CardinalDirection.North,
                                CardinalDirection.West => CardinalDirection.East,
                                _ => nearestDirection
                            };
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
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Back:
                            nearestOrientation = Orientation.Left;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Right:
                            nearestOrientation = Orientation.Back;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Left:
                            nearestOrientation = Orientation.Front;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Up:
                            nearestOrientation = Orientation.Right;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.East,
                                CardinalDirection.East => CardinalDirection.South,
                                CardinalDirection.South => CardinalDirection.West,
                                CardinalDirection.West => CardinalDirection.North,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Down:
                            nearestOrientation = Orientation.Right;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.West,
                                CardinalDirection.East => CardinalDirection.North,
                                CardinalDirection.South => CardinalDirection.East,
                                CardinalDirection.West => CardinalDirection.South,
                                _ => nearestDirection
                            };
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
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Back:
                            nearestOrientation = Orientation.Right;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Right: 
                            nearestOrientation = Orientation.Front;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Left:
                            nearestOrientation = Orientation.Back;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.North,
                                CardinalDirection.East => CardinalDirection.East,
                                CardinalDirection.South => CardinalDirection.South,
                                CardinalDirection.West => CardinalDirection.West,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Up:
                            nearestOrientation = Orientation.Left;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.West,
                                CardinalDirection.East => CardinalDirection.North,
                                CardinalDirection.South => CardinalDirection.East,
                                CardinalDirection.West => CardinalDirection.South,
                                _ => nearestDirection
                            };
                            break;
                        case Orientation.Down:
                            nearestOrientation = Orientation.Left;
                            nearestDirection = nearestDirection switch
                            {
                                CardinalDirection.North => CardinalDirection.East,
                                CardinalDirection.East => CardinalDirection.South,
                                CardinalDirection.South => CardinalDirection.West,
                                CardinalDirection.West => CardinalDirection.North,
                                _ => nearestDirection
                            };
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