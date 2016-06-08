using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float health = 100f;
    public float damage = 1f;
    public float walkDistance = 2f;

    private float damageTime = 0f;
    private float damageOverTime = 0f;

    public ElementType elementType = ElementType.None;

    protected Vector3 startingLocation;
    protected float startingHealth;
    protected float startingSpeed;

    private CastSpell cs = new CastSpell();

    // Use this for initialization
    public virtual void Awake()
    {
        startingLocation = transform.position;
        startingHealth = health;
        startingSpeed = speed;

    }
    public virtual void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(damageTime > 0f)
        {
            health -= (damageOverTime * Time.deltaTime);
            damageTime -= Time.deltaTime;
        }
        //Check if the enemy is dead
        if (health <= 0)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
    public virtual void Reset()
    {
        //Debug.Log("Reset");
        transform.position = startingLocation;
        health = startingHealth;
        speed = startingSpeed;
        gameObject.SetActive(true);
    }

    public virtual void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Spell")
        {
            getHit(col.gameObject.GetComponent<CastSpell>());
        }else if ( col.collider.tag == "Cluster")
        {
            cs.elementType = col.gameObject.GetComponent<ShatterShells>().elementType;
            cs.damageType = col.gameObject.GetComponent<ShatterShells>().damageType;
            cs.damage = col.gameObject.GetComponent<ShatterShells>().damage;
            getHit(cs);
        }
        else if(col.collider.tag == "Pillar")
        {
            getHit(col.gameObject.transform.gameObject.GetComponent<CastSpell>());
        }
        if (col.collider.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().getHit(damage);
        }
    }

    public virtual void OnTriggerEnter(Collider col)
    {

        if (col.GetComponent<Collider>().tag == "Spell")
        {
            getHit(col.gameObject.GetComponent<CastSpell>());
        }
    }

    float endTime = 1;
    float timer = 0;
    public virtual void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<Collider>().tag == "TriggerSpell")
        {
            timer += 1 * Time.deltaTime;

            if(timer > endTime)
            {
                getHit(col.gameObject.GetComponent<CastSpell>());
                timer = 0;
            }
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.collider.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().getHit(damage * Time.deltaTime);
        }
    }

    protected void moveToStartPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, startingLocation, speed * Time.deltaTime);
    }

    protected void getHit(CastSpell spell)
    {
        Debug.Log(spell.elementType);
        if (spell.damageType == DamageType.Instant || spell.damageType == DamageType.InstantDot)
        {
            if (elementType == ElementType.None)     //Normal damage from all element types when the enemy has no element type
            {
                health -= spell.damage;
            }
            else if (elementType == spell.elementType)   //Enemy gets healed when the element type of the damage is the same as his own element type
            {
                health += (spell.damage / 2f);
            }
            else if ((elementType == ElementType.Dark && spell.elementType == ElementType.Light) ||      //Light beats Dark
                (elementType == ElementType.Light && spell.elementType == ElementType.Dark) ||           //Dark beats Light
                (elementType == ElementType.Fire && spell.elementType == ElementType.Air) ||             //Air beats Fire
                (elementType == ElementType.Air && spell.elementType == ElementType.Earth) ||            //Earth beats Air
                (elementType == ElementType.Earth && spell.elementType == ElementType.Ice) ||            //Ice beats Earth
                (elementType == ElementType.Ice && spell.elementType == ElementType.Fire))               //Fire beats Ice
            {
                health -= Mathf.Abs(2 * spell.damage);
            }
            else        //Default normal damage
            {
                health -= spell.damage;
            }
        }

        if(spell.damageType == DamageType.Dot || spell.damageType == DamageType.InstantDot)
        {
            damageTime = spell.dotTimer;

            if (elementType == ElementType.None)     //Normal damage from all element types when the enemy has no element type
            {
                damageOverTime = spell.damage / damageTime;
            }
            else if (elementType == spell.elementType)   //Enemy gets healed when the element type of the damage is the same as his own element type
            {
                if (elementType == ElementType.Light)
                    damageOverTime = ((spell.damage * 2f) / damageTime);
                else
                    damageOverTime = -1f * ((spell.damage / 2f) / damageTime);
                Debug.Log(damageOverTime);
                Debug.Log(damageTime);
            }
            else if ((elementType == ElementType.Dark && spell.elementType == ElementType.Light) ||      //Light beats Dark
                (elementType == ElementType.Light && spell.elementType == ElementType.Dark) ||           //Dark beats Light
                (elementType == ElementType.Fire && spell.elementType == ElementType.Air) ||             //Air beats Fire
                (elementType == ElementType.Air && spell.elementType == ElementType.Earth) ||            //Earth beats Air
                (elementType == ElementType.Earth && spell.elementType == ElementType.Ice) ||            //Ice beats Earth
                (elementType == ElementType.Ice && spell.elementType == ElementType.Fire))               //Fire beats Ice
            {
                damageOverTime = Mathf.Abs((2 * spell.damage) / damageTime);
                if (spell.elementType == ElementType.Light)
                    damageOverTime *= -1f;
            }
            else        //Default normal damage
            {
                damageOverTime = spell.damage / damageTime;
            }
        }
        
        //Check if the enemy is dead
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}