using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class MovementInputHandler : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Vector2 _touchStartPosition;

    public int KeyPadNumber { get; set; }

    [SerializeField] private FloatingJoystick joystick;
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        KeyboardInput();
        // SwipeInput();
        // JoystickInput();
        // KeyPadInput();
    }

    private void KeyPadInput()
    {
        if (KeyPadNumber == 0) return;
        var direction = KeyPadNumber switch
        {
            1 => CardinalDirection.North,
            2 => CardinalDirection.East,
            3 => CardinalDirection.South,
            4 => CardinalDirection.West,
        };
        _playerMovement.TryMove(direction);
    }

    private void JoystickInput()
    {
        var x = (int)joystick.Horizontal;
        var y = (int)joystick.Vertical;
        if (x == 1) _playerMovement.TryMove(CardinalDirection.East);
        if (x == -1) _playerMovement.TryMove(CardinalDirection.West);
        if (y == 1) _playerMovement.TryMove(CardinalDirection.North);
        if (y == -1) _playerMovement.TryMove(CardinalDirection.South);
    }

    private void SwipeInput()
    {
        if (Input.touchCount == 0) return;
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began) _touchStartPosition = touch.position;

        if (touch.phase == TouchPhase.Ended)
        {
            var swipe = touch.position - _touchStartPosition;
            if (swipe.magnitude > 20f) Swipe(swipe);
        }
    }

    private void Swipe(Vector2 swipe)
    {
        var xDelta = Mathf.Abs(swipe.x);
        var yDelta = Mathf.Abs(swipe.y);
        CardinalDirection direction;
        if (xDelta > yDelta) direction = swipe.x > 0 ? CardinalDirection.East : CardinalDirection.West;
        else direction = swipe.y > 0 ? CardinalDirection.North : CardinalDirection.South;
        _playerMovement.TryMove(direction);
    }

    private void KeyboardInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _playerMovement.TryMove(CardinalDirection.North);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            _playerMovement.TryMove(CardinalDirection.South);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _playerMovement.TryMove(CardinalDirection.East);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _playerMovement.TryMove(CardinalDirection.West);
        }
    }
}
