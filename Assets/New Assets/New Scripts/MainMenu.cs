using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (GameManager.Instance != null)
            GameManager.Instance.StartNewGame();
        else
            Debug.LogError("GameManager not found.");
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (GameManager.Instance != null)
            GameManager.Instance.ContinueGame();
        else
            Debug.LogError("GameManager not found.");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Application.Quit();
    }
}