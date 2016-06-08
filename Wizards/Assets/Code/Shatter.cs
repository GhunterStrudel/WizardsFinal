using UnityEngine;
using System.Collections;

public class Shatter : MonoBehaviour 
{
	public int clusterCount;
	public float force = 10.0f;
	public GameObject cluster;

	
	// Update is called once per frame
	void OnCollisionEnter (Collision col) 
	{
		 /*Vector3 forceVec = -col.gameObject.GetComponent<Rigidbody>().velocity.normalized * explosionStrength;
		 foreach(GameObject child in transform)
		 {
		 	child.SetActive(true);
		 	child.GetComponent<Rigidbody>().AddForce(forceVec,ForceMode.Acceleration);
		 }*/
         //target_.rigidbody.AddForce(forceVec,ForceMode.Acceleration);

                  // Calculate Angle Between the collision point and the player
         Vector3 dir = col.contacts[0].point - transform.position;
         // We then get the opposite (-Vector3) and normalize it
         dir = -dir.normalized;
         // And finally we add force in the direction of dir and multiply it by force. 
         // This will push back the player

         for(int i=0;i<clusterCount;i++)
		 {
		 	
		 	GameObject c = Instantiate(cluster, transform.position, Quaternion.identity) as GameObject;
		 	
		 	Physics.IgnoreCollision(c.GetComponent<Collider>(), GetComponent<Collider>());
		 	c.GetComponent<ShatterShells>().cs = GetComponent<CastSpell>();
		 	c.GetComponent<ShatterShells>().clusterCount = clusterCount;
		 	c.GetComponent<Rigidbody>().AddForce(dir*force);
		 }	
	}
}
