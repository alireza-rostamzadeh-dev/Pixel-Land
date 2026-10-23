
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private int gameSceneIndex = 1;
    [SerializeField] private int mainMenuSceneIndex = 0;

    public void Retry()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadSceneAsync(gameSceneIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadSceneAsync(mainMenuSceneIndex);
    }
}
