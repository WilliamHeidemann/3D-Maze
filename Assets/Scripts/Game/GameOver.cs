using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameStarter gameStarter;
    [SerializeField] private PauseController pauseController;
    private bool _isGameOver;
    void Update()
    {
        if (_isGameOver == false) return;
        if (Input.GetKey(KeyCode.Space))
        {
            _isGameOver = false;
            pauseController.enabled = true;
            gameOverScreen.SetActive(false);
            Time.timeScale = 1;
            gameStarter.FirstMaze();
        }
    }

    public void EndGame()
    {
        _isGameOver = true;
        gameOverScreen.SetActive(true);
        pauseController.enabled = false;
        Time.timeScale = 0;
    }
}
