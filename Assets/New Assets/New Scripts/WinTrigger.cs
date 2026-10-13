using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    [Header("UI")]
    public GameObject winPanel;
    public AudioSource winMusic;
    public Rigidbody2D rb;
  
    private bool isWon = false;

    private void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isWon && collision.CompareTag("Player"))
        {
            isWon = true;
            rb.velocity = Vector2.zero;
            AudioListener.pause = true;
            FindObjectOfType<PlayerMovement>().enabled = false;
            FindObjectOfType<PlayerCombat>().enabled = false;
            Time.timeScale = 0;
            // فعال کردن پنل
            if (winPanel != null)
                winPanel.SetActive(true);

            

            if (winMusic != null)
            {
                AudioListener.pause = false; 
                winMusic.Play();
            }

           
            collision.GetComponent<PlayerMovement>().enabled = false;
            collision.GetComponent<PlayerCombat>().enabled = false;
        }
    }

    
    public void RetryLevel()
    {
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        AudioListener.pause = false;
        SceneManager.LoadScene("MainMenu"); 
    }
}
