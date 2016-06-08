using UnityEngine;
using System.Collections;

public class EnvironmentDamage : MonoBehaviour {
    // Use this for initialization
    public float damage;
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("HITTTTT");
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().getHit(damage);
        }
    }
}
