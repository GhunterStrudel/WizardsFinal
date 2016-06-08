using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public CastSpell cs;
	public GameObject cluster;
	float timer;
	public float fireRate;
	public float force;
    public float rot = 90;
	public GameObject barrel;
	// Use this for initialization
	void Start () 
	{
        transform.eulerAngles = new Vector3(270, rot, transform.eulerAngles.z);
        //transform.localEulerAngles = new Vector3(portal.transform.localEulerAngles.x, pRot, portal.transform.localEulerAngles.z);
    }
	
	// Update is called once per frame
	void Update () 
	{
		timer += 1 * Time.deltaTime;
		if(timer > fireRate)
		{	 	
			GameObject c = Instantiate(cluster, barrel.transform.position, barrel.transform.localRotation) as GameObject;
			Physics.IgnoreCollision(c.GetComponent<Collider>(), GetComponent<Collider>());
			c.GetComponent<ShatterShells>().cs = GetComponent<CastSpell>();
			
			c.GetComponent<Rigidbody>().AddForce(Vector3.right * force);

			timer = 0;
		}	
	}
}
