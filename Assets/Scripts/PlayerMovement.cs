using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Dictionary<Square, GameObject> _squarePositions;
    private Square _current;
    private GameObject target;
    public Square ObjectiveSquare;
    public GameObject nextLevelButton;
    public void SetSquares(Dictionary<Square, GameObject> squarePositions, Square startingSquare)
    {
        _squarePositions = squarePositions;
        MoveTo(startingSquare);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && _current.LeftNeighbor.Item2.IsOpen)
        {
            var nextSquare = _current.LeftNeighbor.Item1;
            MoveTo(nextSquare);
        }
        if (Input.GetKeyDown(KeyCode.W) && _current.TopNeighbor.Item2.IsOpen)
        {
            var nextSquare = _current.TopNeighbor.Item1;
            MoveTo(nextSquare);
        }
        if (Input.GetKeyDown(KeyCode.D) && _current.RightNeighbor.Item2.IsOpen)
        {
            var nextSquare = _current.RightNeighbor.Item1;
            MoveTo(nextSquare);
        }
        if (Input.GetKeyDown(KeyCode.S) && _current.BottomNeighbor.Item2.IsOpen)
        {
            var nextSquare = _current.BottomNeighbor.Item1;
            MoveTo(nextSquare);
        }
        
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.1f);
    }

    private void MoveTo(Square nextSquare)
    {
        _current = nextSquare;
        var mazeRotator = FindObjectOfType<MazeRotator>();
        mazeRotator.Rotate(nextSquare.FaceDirection);
        target = _squarePositions[nextSquare];
        if (nextSquare == ObjectiveSquare) nextLevelButton.SetActive(true);
    }
}
