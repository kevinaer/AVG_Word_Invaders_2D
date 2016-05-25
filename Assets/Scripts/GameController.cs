using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
    public Text scoreText;
    public Text gameStatusText;
    public GameObject restartHUD;
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
        StartCoroutine(GameOverSetUp());
    }

    IEnumerator GameOverSetUp()
    {
        yield return new WaitForSeconds(1);
        gameStatusText.text = "GAME OVER!";
        restartHUD.SetActive(true);
    }

    public bool isGameOver()
    {
        return gameOver;
    }
}
