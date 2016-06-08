using UnityEngine;
using System.Collections;

public class Icicle : MonoBehaviour {

    bool fallen = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Spell" || collision.gameObject.tag == "Player" && !fallen)
        {
            fallen = true;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
