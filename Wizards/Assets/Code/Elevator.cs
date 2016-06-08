using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

    public int speed;
    public GameObject startLocation;
    public GameObject endLocation;
    Vector3 destination;
    bool standingOnElevator;
    float moveTimer = 1.0f;

	void Start () {
        destination = startLocation.transform.position;
	}
	
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        if (standingOnElevator)
        {
            moveTimer -= Time.deltaTime;
        }
        if (moveTimer <= 0.0f)
        {
            changeDestination();
        }	
	}

    void OnCollisionEnter(Collision collision)
    {
        standingOnElevator = true;
    }

    void OnCollisionExit(Collision collision)
    {
        standingOnElevator = false;
        moveTimer = 1.0f;
    }

    void changeDestination()
    {
        if (destination == transform.position)
        {
            if (destination == startLocation.transform.position)
            {
                Debug.Log("up");
                destination = endLocation.transform.position;
            }
            else if (destination == endLocation.transform.position)
            {
                Debug.Log("down");
                destination = startLocation.transform.position;
            }
        }
    }
}
