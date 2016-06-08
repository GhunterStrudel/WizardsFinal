using UnityEngine;

public class Wander : Enemy {
    bool direction = true;
    bool playerInRange = false;
    bool goBackToStart = false;

    private Vector3 playerLocation;
     public GameObject sprite;
     float startScale;
     int flip;    // Use this for initialization
    public override void Start () {
        base.Start();
        startScale = sprite.transform.localScale.x;
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();

        if (playerInRange)
            moveToPlayer();
        else if (goBackToStart)
        {
            if (transform.position == startingLocation)
            {
                goBackToStart = false;
            }
            else
            {
                moveToStartPosition();
            }        
        }
        else
            move();
    }

    public override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);

        if (col.collider.tag != "Player")
            direction = !direction;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerLocation = other.transform.position;
            playerInRange = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            playerLocation = other.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
            goBackToStart = true;
        }
    }

    void move()
    {
        if (direction)
        {
            if (transform.position.x - startingLocation.x >= walkDistance)
                direction = false;

                flip = 1;
           transform.Translate(speed * Time.deltaTime, 0, 0);
        }
        else
        {
            if (startingLocation.x - transform.position.x >= walkDistance)
                direction = true;

                 flip = -1;
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }

        sprite.transform.localScale = new Vector3(startScale*flip, sprite.transform.localScale.y, sprite.transform.localScale.z);
    }

    void moveToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerLocation, speed * Time.deltaTime);
        
        if(Vector3.Dot(transform.position, playerLocation) < 0)
            flip = -1;
        else
            flip = 1;

        sprite.transform.localScale = new Vector3(startScale*flip, sprite.transform.localScale.y, sprite.transform.localScale.z);

    }
}
