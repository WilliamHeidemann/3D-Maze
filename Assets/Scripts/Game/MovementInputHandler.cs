using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class MovementInputHandler : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    
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
