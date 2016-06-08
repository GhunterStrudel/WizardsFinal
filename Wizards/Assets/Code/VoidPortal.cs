using UnityEngine;
using System.Collections;

public class VoidPortal : MonoBehaviour 
{
	public GameObject portal;
	public ParticleSystem particle;
	public float startX;
	public float startZ;
	public float rot;
	public float pRot = -144.3947f;
	
	// Update is called once per frame
	void Start()
	{
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, rot, transform.eulerAngles.z);
		portal.transform.localEulerAngles = new Vector3(portal.transform.localEulerAngles.x, pRot, portal.transform.localEulerAngles.z);
	}

	void Update () 
	{

		if(startZ < 1)
		{
			if(startX < 1.20f)
			{
				startX += 5 * Time.deltaTime;
				if(startX > 1.20f)
					startX = 1.20f;
			}
			else
			{
				startZ += 5 * Time.deltaTime;
				if(startZ > 1)
				{
					startZ = 1;
					GetComponent<Collider>().enabled = true;
					particle.Play();
					GetComponent<CastSpell>().enabled = true;
				}
			}
		}

		portal.transform.localScale = new Vector3(startX, 1f, startZ);
	}

	void OnTriggerStay(Collider coll)
	{
		if(coll.gameObject.tag == "Enemy" && coll.gameObject.GetComponent<Enemy>().health < GetComponent<CastSpell>().damage)
		{
			coll.transform.position = Vector3.MoveTowards(coll.transform.position, transform.position, 0.1f);
			coll.transform.parent = transform;
			coll.transform.GetComponent<Rigidbody>().isKinematic = true;
			if(coll.transform.localScale.x > 0)
				coll.transform.localScale -= new Vector3(coll.transform.localScale.z/100f,coll.transform.localScale.z/100f,0f);
		}
	}
}
