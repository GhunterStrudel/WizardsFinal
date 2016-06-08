using UnityEngine;
using System.Collections;

public class CastSpell : MonoBehaviour
{
	public ParticleSystem ps;
	public Material mat;

	public int spellID;
	public string name;
	public float damage;
    public ElementType elementType;
	public CastType castType;
	public DamageType damageType;
	public float dotTimer;
	public bool aoe;
	public float aoeRange;
	public bool bouncing;
    public bool cluster;
	public Color baseColor;
	public Texture effectTexture;
    public float lifeTime;
    public Cast caster;

    public Vector3 target;
    public Transform targetObj;
    public Quaternion targetRotation;
	int initSpeed = 1000;
    bool explode;

	// Use this for initialization
	void Start () 
	{
		ps.GetComponent<Renderer>().material.mainTexture = effectTexture;
		ps.GetComponent<Renderer>().material.SetColor("_TintColor", baseColor);
        
         //ps.GetComponent<ParticleSystem>().startColor = baseColor;
         if(bouncing)
            initSpeed = 750;

         switch (castType)
        {
            case CastType.Direction: GetComponent<Rigidbody>().AddRelativeForce (Vector3.right * initSpeed); break;//transform.Translate(Vector3.right * 10 * Time.deltaTime); break;//
            case CastType.Target: 
                                    transform.position = target; 

                                    if(damageType != DamageType.Shield)
                                    {
                                        GetComponent<Collider>().isTrigger = true; 
                                        Destroy(gameObject.GetComponent<Rigidbody>()); 
                                    }
                                    else
                                    {
                                        try{ GetComponent<Renderer>().material.color = baseColor;}catch{}
                                        transform.parent = targetObj;
                                       
                                    }
            break;
            case CastType.Uproot: transform.position = target; transform.GetChild(0).GetComponent<Renderer>().material.color = baseColor; break;
            case CastType.Summon: 
                                    try{ GetComponent<Renderer>().material.color = baseColor; } catch{} 

            break;
            case CastType.Channel:
                    var sh = ps.shape;
                    sh.shapeType = ParticleSystemShapeType.Box;
                ps.startSpeed = 10;
                break;
        }
	}

    void OnCollisionEnter(Collision collider)
    {
        if(!bouncing)
        {
            if (castType == CastType.Direction)
            {
                if(aoe)
                {
                    Explosion();
                }
                else
                {
                   KillSpell(); 
                }  
            }
        }

        
    }

    float timer = 0;
    void Update()
    {
        if (castType == CastType.Target)
        {
            transform.position = targetObj.position; 
        }
        //check if particle is done emitting
         if (!ps.IsAlive())
            Destroy(this.gameObject);

        timer += 1 * Time.deltaTime;

        if(lifeTime > 0)       
            if(timer > lifeTime)
                if(castType == CastType.Channel)    
                    StopSpell();
                else     
                    KillSpell();  
    }

    public void StopEmit()
    {
        ps.Stop();

        if(damageType != DamageType.Shield)
        {
            if(GetComponent<Collider>())
                Destroy(GetComponent<Collider>());

            if(GetComponent<Rigidbody>())
                Destroy(GetComponent<Rigidbody>());

             if(GetComponent<Renderer>())
                 GetComponent<Renderer>().enabled = false;
        }
    }

    bool once = false;
    public void KillSpell()
    {

         ps.startSpeed = 1;
                ps.loop = false;

                if(GetComponent<Rigidbody>())
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    
                ps.emissionRate = 0;
                ps.startSize = 2;
                if(!once && gameObject.tag != "VoidTrigger")
                {
                    once = true;
                    ps.Emit(10);
                }

        StopEmit();
    }

    public void Explosion()
    {
        explode = true;

        if(GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().velocity = Vector3.zero;

        GetComponent<SphereCollider>().isTrigger = true;
        GetComponent<SphereCollider>().radius = aoeRange;

        ps.startSize = aoeRange;
        ps.startLifetime = 2;
        ps.Emit(100);
    }

    public void StopSpell()
    {
        StopEmit();
        caster.casting = false;
    }
}
