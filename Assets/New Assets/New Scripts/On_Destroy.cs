using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_Destroy : MonoBehaviour
{
    public GameObject wall_1;
    public GameObject wall_2;

    private void OnDestroy()
    {
        if (wall_1 != null)
            wall_1.SetActive(false);

        if (wall_2 != null)
            wall_2.SetActive(false);

        Debug.Log("Walls deactivated because object was destroyed");
    }
}
