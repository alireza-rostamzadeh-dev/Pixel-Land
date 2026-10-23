
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject winPanel;
    public AudioSource winMusic;
    public Rigidbody2D rb;

    private bool isWon;

    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWon || !collision.CompareTag("Player"))
            return;

        isWon = true;

        playerMovement = collision.GetComponent<PlayerMovement>();
        playerCombat = collision.GetComponent<PlayerCombat>();

        if (rb != null)
            rb.velocity = Vector2.zero;

        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

        if (playerRb != null)
            playerRb.velocity = Vector2.zero;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (playerCombat != null)
            playerCombat.enabled = false;

        GameEvents.OnLevelCompleted.Invoke();

        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
        AudioListener.pause = true;

        if (winMusic != null)
        {
            AudioListener.pause = false;
            winMusic.ignoreListenerPause = true;
            winMusic.Play();
        }
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu");
    }
}
