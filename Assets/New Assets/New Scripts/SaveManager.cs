using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string Key = "B09ks9mLUVCrSIEylBlY8PS2wyrXbonF5JrGHDG8ksc=";
    private const string IV = "B8YvQrG88QoLlV6ru8RBTQ==";

    public string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void SaveGame()
    {
        PlayerStats stats = FindObjectOfType<PlayerStats>();

        if (stats == null)
        {
            Debug.LogError("Save failed: PlayerStats not found.");
            return;
        }

        GameState state = new GameState
        {
            currentScene = SceneManager.GetActiveScene().name,
            playerPosition = stats.transform.position,
            currentHealth = stats.currentHealth,
            currentMana = stats.currentMana
        };

        SaveComponent[] components =
            FindObjectsOfType<SaveComponent>(true);

        foreach (SaveComponent component in components)
            component.Save(state);

        try
        {
            string json = JsonUtility.ToJson(state, true);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] encryptedData = EncryptData(data);

            File.WriteAllBytes(SavePath, encryptedData);

            Debug.Log("Game saved: " + SavePath);
        }
        catch (Exception exception)
        {
            Debug.LogError("Save failed: " + exception);
        }
    }

    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogError("Save file not found: " + SavePath);
            return;
        }

        try
        {
            byte[] encryptedData = File.ReadAllBytes(SavePath);
            byte[] decryptedData = DecryptData(encryptedData);
            string json = Encoding.UTF8.GetString(decryptedData);

            GameState state = JsonUtility.FromJson<GameState>(json);

            if (state == null)
            {
                Debug.LogError("Save data is invalid.");
                return;
            }

            PlayerStats stats = FindObjectOfType<PlayerStats>();

            if (stats == null)
            {
                Debug.LogError("Load failed: PlayerStats not found.");
                return;
            }

            stats.transform.position = state.playerPosition;
            stats.currentHealth =
                Mathf.Clamp(state.currentHealth, 0f, stats.maxHealth);
            stats.currentMana =
                Mathf.Clamp(state.currentMana, 0f, stats.maxMana);

            SaveComponent[] components =
                FindObjectsOfType<SaveComponent>(true);

            foreach (SaveComponent component in components)
                component.Load(state);

            GameEvents.HealthChanged(
                stats.currentHealth, stats.maxHealth);

            Debug.Log(
                "Game loaded. Position: " + state.playerPosition);
        }
        catch (Exception exception)
        {
            Debug.LogError("Load failed: " + exception);
        }
    }

    private byte[] EncryptData(byte[] data)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(Key);
            aes.IV = Convert.FromBase64String(IV);

            using (MemoryStream stream = new MemoryStream())
            using (CryptoStream crypto = new CryptoStream(
                stream, aes.CreateEncryptor(),
                CryptoStreamMode.Write))
            {
                crypto.Write(data, 0, data.Length);
                crypto.FlushFinalBlock();
                return stream.ToArray();
            }
        }
    }

    private byte[] DecryptData(byte[] data)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String(Key);
            aes.IV = Convert.FromBase64String(IV);

            using (MemoryStream input = new MemoryStream(data))
            using (CryptoStream crypto = new CryptoStream(
                input, aes.CreateDecryptor(),
                CryptoStreamMode.Read))
            using (MemoryStream output = new MemoryStream())
            {
                crypto.CopyTo(output);
                return output.ToArray();
            }
        }
    }
}