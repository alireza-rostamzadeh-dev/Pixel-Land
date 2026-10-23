
using UnityEngine;

public class DeathPoint : MonoBehaviour
{
    public GameObject wall_1;
    public GameObject wall_2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (wall_1 != null)
            wall_1.SetActive(true);

        if (wall_2 != null)
            wall_2.SetActive(true);
    }
}
