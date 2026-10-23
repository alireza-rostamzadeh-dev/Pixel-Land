
using System.Collections;
using UnityEngine;

public class CampfireInteraction : MonoBehaviour
{
    public GameObject interactionUI;
    public GameObject player;

    private bool isPlayerInRange;
    private bool isInteracting;
    private PlayerStats playerStats;
    private PlayerItems playerItems;
    private ScreenFader screenFader;

    private void Awake()
    {
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            playerItems = player.GetComponent<PlayerItems>();
        }

        screenFader = FindObjectOfType<ScreenFader>();
    }

    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && !isInteracting &&
            Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInRange = true;

        if (player == null)
        {
            player = other.gameObject;
            playerStats = player.GetComponent<PlayerStats>();
            playerItems = player.GetComponent<PlayerItems>();
        }

        if (interactionUI != null)
            interactionUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInRange = false;

        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    private void Interact()
    {
        if (playerStats == null)
            return;

        isInteracting = true;

        playerStats.currentHealth = playerStats.maxHealth;
        GameEvents.HealthChanged(
            playerStats.currentHealth,
            playerStats.maxHealth
        );

        if (playerItems != null)
            playerItems.ResetPotions();

        GameEvents.OnCampfireActivated.Invoke(gameObject);

        if (screenFader != null)
            StartCoroutine(PlayCampfireEffect());
        else
            isInteracting = false;
    }

    private IEnumerator PlayCampfireEffect()
    {
        yield return StartCoroutine(screenFader.FadeOutIn(2f));
        isInteracting = false;
    }
}
