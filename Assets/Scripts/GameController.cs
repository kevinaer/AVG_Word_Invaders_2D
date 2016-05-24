using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
    public Text scoreText;
    public Text gameStatusText;
    private int score;
    private bool gameOver = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void AddScore(int increment)
    {
        score += increment;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOver = true;
        gameStatusText.text = "GAME OVER!";
    }

    public bool isGameOver()
    {
        return gameOver;
    }
}
