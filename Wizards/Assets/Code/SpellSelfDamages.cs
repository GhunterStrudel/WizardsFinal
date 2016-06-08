using UnityEngine;
using System.Collections;

public class SpellSelfDamages : MonoBehaviour 
{

	public Player player;
	private float damageTime = 0f;
    private float damageOverTime = 0f;
    private CastSpell cs = new CastSpell();

    public void Update()
    {
	if(damageTime > 0)
        {
            player.currenthealth -= (damageOverTime * Time.deltaTime);
            damageTime -= Time.deltaTime;
        }
    }

	public void OnCollisionEnter(Collision col)
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
        /*else if(col.collider.tag == "Pillar")
        {
            getHit(col.gameObject.transform.parent.gameObject.GetComponent<CastSpell>());
        }*/
    }

    public  void OnTriggerEnter(Collider col)
    {

        if (col.GetComponent<Collider>().tag == "Spell")
        {
            getHit(col.gameObject.GetComponent<CastSpell>());
        }
    }

    float endTime = 1;
    float timer = 0;
    public void OnTriggerStay(Collider col)
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

    protected void getHit(CastSpell spell)
    {
        Debug.Log(spell.damageType);
        if (spell.damageType == DamageType.Instant || spell.damageType == DamageType.InstantDot)
        {           
            player.currenthealth -= spell.damage;       
        }

        if(spell.damageType == DamageType.Dot || spell.damageType == DamageType.InstantDot)
        {
            damageTime = spell.dotTimer;
            damageOverTime = spell.damage / damageTime;  
        }
    }
}
