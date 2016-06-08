using UnityEngine;
using System.Collections;

public class BasicWander : Enemy
{
    bool direction = true;
    int flip;
    float startScale;
    public GameObject sprite;
    // Use this for initialization
    public override void Start () {
        base.Start();	
        startScale = sprite.transform.localScale.x;
	}

    // Update is called once per frame
    public override void Update ()
    {
        base.Update();

        move();
    }
    public override void OnCollisionEnter(Collision col)
    {
        base.OnCollisionEnter(col);

        if (col.collider.tag != "Player")
            direction = !direction;
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
}
