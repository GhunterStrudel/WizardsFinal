using UnityEngine;
using System.Collections;

public class ShatterShells : MonoBehaviour {

	public CastSpell cs;
	public ElementType elementType;
	public DamageType damageType;
	public float damage;
    float lifeTime;
    public ParticleSystem ps;
    public int clusterCount;
	// Use this for initialization
	void Start () 
	{
		elementType = cs.elementType;
		damageType = cs.damageType;

		lifeTime = cs.lifeTime / 2.5f;

		if(clusterCount > 0)
			damage = cs.damage / clusterCount;
		else
			damage = cs.damage;

		ps.GetComponent<Renderer>().material.mainTexture = cs.effectTexture;
		ps.GetComponent<Renderer>().material.SetColor("_TintColor", cs.baseColor);
	}

	float timer;

	void Update()
	{
		timer += 1 * Time.deltaTime;

		if(timer > 0.1f)
			GetComponent<Collider>().enabled = true;

		if(timer > lifeTime)
			Destroy(this.gameObject);

			//Physics.IgnoreLayerCollision(11,11, true);
	}

	void OnCollisionEnter(Collision collider)
	{
		if(collider.gameObject.tag == "Player" || collider.gameObject.tag == "Enemy")
			Destroy(this.gameObject);
	}
}
