using UnityEngine;
using System.Collections;
using Assets.Code;

public class Windmill : Switch {

	public float maxSpeed;
	 float speed;

	// Use this for initialization
	void Start () {
	
	}

	void Update()
	{
		transform.Rotate(Vector3.back * speed * Time.deltaTime);
		isHit = speed > 800;
	}
	
	public void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<Collider>().tag == "TriggerSpell")
        {
            if(speed < maxSpeed)
            	speed += 350 * Time.deltaTime;
        }
    }
}
