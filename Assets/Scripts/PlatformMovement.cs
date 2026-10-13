using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{   
    [SerializeField] private GameObject[] waypoints;
    private int currentwaypointindex = 0;

    [SerializeField] private float speed = 2;
    
    private void Update()
    {
        if(Vector2.Distance(waypoints[currentwaypointindex].transform.position , transform.position) < 1){
            currentwaypointindex++;
            if(currentwaypointindex >= waypoints.Length)
            {
                currentwaypointindex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position , waypoints[currentwaypointindex].transform.position , Time.deltaTime * speed);
    }
}
