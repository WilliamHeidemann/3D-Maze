using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class MovementInputHandler : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private Vector2 _touchStartPosition;
        private readonly List<CardinalDirection> _movementInput = new();
        private readonly InputStructure _inputStructure = new();
        
        void Start()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        void Update()
        {
            // HandleMovementQueue();
            // if (_movementInput.Count > 0)
            // {
            //     _playerMovement.TryMove(_movementInput.First());
            // }
            
            HandleInput();
            _playerMovement.DetermineInput(_inputStructure.GetInput());

            // KeyboardInput();
            // SwipeInput();
            // JoystickInput();
            // KeyPadInput();
        }

        private class InputStructure
        {
            private readonly List<CardinalDirection> _list = new();
            private readonly HashSet<CardinalDirection> _set = new();

            public void Add(CardinalDirection direction)
            {
                if (_set.Contains(direction)) return;
                _set.Add(direction);
                _list.Add(direction);
            }

            public void Remove(CardinalDirection direction)
            {
                _set.Remove(direction);
                _list.Remove(direction);
            }
            
            public List<CardinalDirection> GetInput() => _list;
        }

        // Requirements 
        // Holding down a key will move in that direction 
        // If multiple keys are pressed, a direction towards a non-wall should have higher priority
        // If multiple keys are pressed, the ball will move in the direction that was pressed first
        
        // Solution
        // A data structure that captures inputs
        // It contains 0-4 cardinal directions ordered from first pressed to last
        // The first direction that is not closed by a wall will be the square the player should move towards
        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) _inputStructure.Add(CardinalDirection.North);
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) _inputStructure.Add(CardinalDirection.East);
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) _inputStructure.Add(CardinalDirection.South);
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) _inputStructure.Add(CardinalDirection.West);
            
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) _inputStructure.Remove(CardinalDirection.North);
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) _inputStructure.Remove(CardinalDirection.East);
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) _inputStructure.Remove(CardinalDirection.South);
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) _inputStructure.Remove(CardinalDirection.West);
        }
        
        
        private void HandleMovementQueue()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                _movementInput.Add(CardinalDirection.North);
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
                _movementInput.RemoveAll(c => c == CardinalDirection.North);
            
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                _movementInput.Add(CardinalDirection.East);
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
                _movementInput.RemoveAll(c => c == CardinalDirection.East);

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                _movementInput.Add(CardinalDirection.South);
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
                _movementInput.RemoveAll(c => c == CardinalDirection.South);
            
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                _movementInput.Add(CardinalDirection.West);
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
                _movementInput.RemoveAll(c => c == CardinalDirection.West);
        }

        // private void KeyboardInput()
        // {
        //     if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //     {
        //         _playerMovement.TryMove(CardinalDirection.North);
        //     }
        //     if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //     {
        //         _playerMovement.TryMove(CardinalDirection.South);
        //     }
        //     if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //     {
        //         _playerMovement.TryMove(CardinalDirection.East);
        //     }
        //     if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //     {
        //         _playerMovement.TryMove(CardinalDirection.West);
        //     }
        // }
    }
}
