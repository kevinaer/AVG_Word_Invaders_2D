using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class StartScreenButtonManager : MonoBehaviour {
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void StartMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
