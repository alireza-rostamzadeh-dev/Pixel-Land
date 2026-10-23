
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string gameplaySceneName = "Scene 1";

    private bool shouldLoadSave;
    private bool shouldStartNewGame;
    private bool isLoadingScene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        Instance = null;
    }

    public void SaveGame()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveGame();
        else
            Debug.LogError("SaveManager not found.");
    }

    public void StartNewGame()
    {
        if (isLoadingScene)
            return;

        shouldLoadSave = false;
        shouldStartNewGame = true;

        if (SceneManager.GetActiveScene().name == gameplaySceneName)
        {
            shouldStartNewGame = false;
            GameEvents.GameStart.Invoke();
            return;
        }

        LoadGameplayScene();
    }

    public void ContinueGame()
    {
        if (isLoadingScene)
            return;

        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager not found.");
            return;
        }

        if (!File.Exists(SaveManager.Instance.SavePath))
        {
            Debug.LogWarning("No save file found.");
            return;
        }

        shouldStartNewGame = false;
        shouldLoadSave = true;

        if (SceneManager.GetActiveScene().name == gameplaySceneName)
        {
            StartCoroutine(LoadSaveAfterSceneReady());
            return;
        }

        LoadGameplayScene();
    }

    private void LoadGameplayScene()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        isLoadingScene = true;

        SceneManager.LoadSceneAsync(gameplaySceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameEvents.OnSceneLoaded.Invoke();

        if (scene.name != gameplaySceneName)
            return;

        isLoadingScene = false;

        if (shouldLoadSave)
        {
            shouldLoadSave = false;
            StartCoroutine(LoadSaveAfterSceneReady());
            return;
        }

        if (shouldStartNewGame)
        {
            shouldStartNewGame = false;
            GameEvents.GameStart.Invoke();
        }
    }

    private IEnumerator LoadSaveAfterSceneReady()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        if (SaveManager.Instance != null)
            SaveManager.Instance.LoadGame();
        else
            Debug.LogError("SaveManager not found after scene load.");
    }
}
