using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireInteraction : MonoBehaviour
{
    public GameObject interactionUI;
    private bool isPlayerInRange;
    public GameObject player;
    

    void Start()
    {
        interactionUI.SetActive(false);
    }


    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactionUI.SetActive(true);
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            interactionUI.SetActive(false);
            isPlayerInRange = false;
        }
    }

    private void Interact()
{
    PlayerStats stats = player.GetComponent<PlayerStats>();
    PlayerItems items = player.GetComponent<PlayerItems>();

    
    
    StartCoroutine(FindObjectOfType<ScreenFader>().FadeOutIn(2f));
    
    stats.currentHealth = stats.maxHealth;
    stats.currentMana = stats.maxMana;
    
    // statsUI.SetHealth(stats.currentHealth);
    // statsUI.SetMana(stats.currentMana);
    
    if (items != null)
        {
            items.ResetPotions();
        }
}

}
