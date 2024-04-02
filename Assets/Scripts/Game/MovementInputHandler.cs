using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class MovementInputHandler : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        private readonly InputStructure inputStructure = new();
        private readonly MobileInputStructure mobileInputStructure = new();
        
        void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            HandleInput();
            playerMovement.CalculateNearest();
            playerMovement.DetermineInput(inputStructure.GetInput());
            // mobileInputStructure.Update();
            // playerMovement.DetermineInput(mobileInputStructure.GetInput());
        }
        
        // Requirements 
        // Holding down a key will move in that direction 
        // If multiple keys are pressed, a direction towards a non-wall should have higher priority
        // If multiple keys are pressed, the ball will move in the direction that was pressed first
        
        // Solution
        // A data structure that captures inputs
        // It contains 0-4 cardinal directions ordered from first pressed to last
        // The first direction that is not closed by a wall will be the square the player should move towards
        private interface IInputHandler
        {
            public List<CardinalDirection> GetInput();
        }
        
        private class InputStructure : IInputHandler
        {
            private readonly List<CardinalDirection> list = new();
            private readonly HashSet<CardinalDirection> set = new();

            public void Add(CardinalDirection direction)
            {
                if (set.Contains(direction)) return;
                set.Add(direction);
                list.Add(direction);
            }

            public void Remove(CardinalDirection direction)
            {
                set.Remove(direction);
                list.Remove(direction);
            }
            
            public List<CardinalDirection> GetInput() => list;
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) inputStructure.Add(CardinalDirection.North);
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) inputStructure.Add(CardinalDirection.East);
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) inputStructure.Add(CardinalDirection.South);
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) inputStructure.Add(CardinalDirection.West);
            
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) inputStructure.Remove(CardinalDirection.North);
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) inputStructure.Remove(CardinalDirection.East);
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) inputStructure.Remove(CardinalDirection.South);
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) inputStructure.Remove(CardinalDirection.West);
        }
        private class MobileInputStructure : IInputHandler
        {
            private readonly List<CardinalDirection> list = new();
            private readonly Dictionary<CardinalDirection, float> dictionary = new()
            {
                { CardinalDirection.North, 0.0f },
                { CardinalDirection.South, 0.0f },
                { CardinalDirection.East, 0.0f },
                { CardinalDirection.West, 0.0f },
            };

            public void Update()
            {
                var acceleration = Input.acceleration;
                dictionary[CardinalDirection.North] = acceleration.y;
                dictionary[CardinalDirection.South] = -acceleration.y;
                dictionary[CardinalDirection.East] = acceleration.x;
                dictionary[CardinalDirection.West] = -acceleration.x;
            }

            public List<CardinalDirection> GetInput() => 
                dictionary
                    .Where(pair => pair.Value > 0.4f)
                    .OrderByDescending(pair => pair.Value)
                    .Select(pair => pair.Key)
                    .ToList();
        }
    }
}
