
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private string mainMenuSceneName = "Main Menu";

    private bool isPaused;

    private void Awake()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if (isPaused)
            CancelPause();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (isPaused || pausePanel == null)
            return;

        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

        GameEvents.OnGamePaused.Invoke();
    }

    public void SaveGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveGame();
            return;
        }

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame();
            return;
        }

        Debug.LogError("SaveManager not found.");
    }

    public void CancelPause()
    {
        if (!isPaused)
            return;

        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        GameEvents.OnGameResumed.Invoke();
    }

    public void QuitGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        SceneManager.LoadSceneAsync(mainMenuSceneName);
    }

    private void OnDisable()
    {
        if (!isPaused)
            return;

        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        GameEvents.OnGameResumed.Invoke();
    }
}
