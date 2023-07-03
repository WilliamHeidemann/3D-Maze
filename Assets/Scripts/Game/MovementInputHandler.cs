using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class MovementInputHandler : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private Vector2 _touchStartPosition;
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
        TouchInput();
    }

    private void TouchInput()
    {
        if (Input.touchCount == 0) return;
        var touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            _touchStartPosition = touch.position;
        }

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
        if (xDelta > yDelta)
        {
            direction = swipe.x > 0 ? CardinalDirection.East : CardinalDirection.West;
        }
        else
        {
            direction = swipe.y > 0 ? CardinalDirection.North : CardinalDirection.South;
        }
        _playerMovement.TryMove(direction);
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _playerMovement.TryMove(CardinalDirection.North);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _playerMovement.TryMove(CardinalDirection.South);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _playerMovement.TryMove(CardinalDirection.East);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _playerMovement.TryMove(CardinalDirection.West);
        }
    }
}
