using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public int speed;
    public GameObject player;
    public GameObject startLocation;
    public GameObject endLocation;
    Vector3 destination;

    void Start () {
        destination = startLocation.transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        player.transform.parent = gameObject.transform;
    }

    void OnCollisionExit(Collision collision)
    {
        player.transform.parent = null;
    }

    void Update () {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        if (destination == transform.position)
        {
            if (destination == startLocation.transform.position)
            {
                destination = endLocation.transform.position;
            }
            else if (destination == endLocation.transform.position)
            {
                destination = startLocation.transform.position;
            }
        }
    }
}
