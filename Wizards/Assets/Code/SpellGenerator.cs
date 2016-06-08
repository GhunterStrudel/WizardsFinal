using UnityEngine;
using System.Collections;
using System;

public class SpellGenerator : MonoBehaviour 
{
	public Spell spellA;
	public Spell spellB;

	public SpellBook sb;
    public GameObject player;
    public GameObject spellPrefab;
    public GameObject stonePillar;
    public GameObject cloud;
    public GameObject channel;
    public GameObject shatter;
    public GameObject cannon;
    public GameObject voidPortal;
    public GameObject lightShield;
    public DrawLine dl;
    bool setspell;

    void Update()
    {
        if(WiimoteStatus.WiiEnabled)
            setspell = WiimoteStatus.buttonB;
        else
            setspell = Input.GetKey(KeyCode.Q);

        if (!setspell)
        {   
            if(spellA.effectTexture != null)
                Set();
            //dl.Reset();
        }
    }
	
	// Update is called once per frame
	public void Set () 
	{
		if(spellB.effectTexture != null)
        { 
			Generate(spellA, spellB);
			spellA = new Spell();
			spellB = new Spell();
		}
	    else
		{
			Generate(spellA);
			spellA = new Spell();
		}
	}

	void Generate(Spell spellA)
	{
        GameObject s = Instantiate(SetSpellObject(spellA), player.transform.position,Quaternion.identity) as GameObject;

        CastSpell cs = s.GetComponent<CastSpell>();

        cs.damage = spellA.damage;
        cs.name = spellA.name;
        cs.elementType = spellA.elementType;
		cs.castType = spellA.castType;
		cs.damageType = spellA.damageType;
		cs.dotTimer = spellA.dotTimer;
		cs.aoe = spellA.aoe;
		cs.aoeRange = spellA.aoeRange;
		cs.bouncing = spellA.bouncing;
		cs.baseColor = spellA.baseColor;
		cs.effectTexture = spellA.effectTexture;
		cs.lifeTime = spellA.lifeTime;
        cs.castType = spellA.castType;
        cs.damageType = spellA.damageType;
        cs.dotTimer = spellA.dotTimer;
        cs.aoe = spellA.aoe;
        cs.aoeRange = spellA.aoeRange;
        cs.bouncing = spellA.bouncing;
        cs.cluster = spellA.cluster;
        cs.baseColor = spellA.baseColor;
        cs.effectTexture = spellA.effectTexture;
        player.GetComponent<Cast>().CastAble(s);
        player.GetComponent<Cast>().castEffect.startColor = cs.baseColor;
    }


	void Generate(Spell spellA, Spell spellB)
	{
        GameObject s = Instantiate(SetSpellObject(spellB), player.transform.position,Quaternion.identity) as GameObject;

        CastSpell cs = s.GetComponent<CastSpell>();

        cs.name = spellA.name + "" + spellB.name;
		cs.damage = spellA.damage + spellB.damage;
		cs.elementType = spellA.elementType;
		cs.castType = spellB.castType;

		if(spellA.damageType == spellB.damageType)
			cs.damageType = spellA.damageType;
		else if(spellB.damageType == DamageType.Shield)
            cs.damageType = spellB.damageType;
        else
			cs.damageType = DamageType.InstantDot;

		cs.dotTimer = spellA.dotTimer + spellB.dotTimer;
		cs.aoe = spellB.aoe;
		cs.aoeRange = spellA.aoeRange + spellB.aoeRange;
		cs.bouncing = spellB.bouncing;
        cs.cluster = spellB.cluster;
		cs.baseColor = spellA.baseColor;
		cs.effectTexture = spellA.effectTexture;
		cs.lifeTime = spellB.lifeTime;

        player.GetComponent<Cast>().CastAble(s);
        player.GetComponent<Cast>().castEffect.startColor = cs.baseColor;
	}

	public bool RetrieveSpell(string id, float score)
    {
        if (player.GetComponent<Player>().canCast(sb.spells[Convert.ToInt32(id)].manaCost) && sb.spells[Convert.ToInt32(id)].unlocked)
        {
            if (spellA.effectTexture == null)
                spellA = sb.spells[Convert.ToInt32(id)];
            else
                spellB = sb.spells[Convert.ToInt32(id)];

            return true;
        }
        else
        {
            //player.GetComponent<Player>().getHit(sb.spells[Convert.ToInt32(id)].damage);
            return false;
        }

	}

    public void FailSpell(float score)
    {
        player.GetComponent<Player>().failSpell(score);
    }

    public GameObject SetSpellObject(Spell s)
    {
         GameObject obj = null;

        switch(s.castType)
        {
            case CastType.Summon: 

                if(s.elementType == ElementType.Air)
                    obj = cloud;
                else if (s.elementType == ElementType.Earth)
                {
                    if (s.spellID == 401)
                        obj = stonePillar;
                    else
                        obj = cannon;
                }
                   
                else if (s.elementType == ElementType.Dark)
                    obj = voidPortal;

                break;

            case CastType.Channel: obj = channel; break;
            case CastType.Target: 
                                    if(s.damageType == DamageType.Shield)
                                        obj = lightShield;
                                    else
                                        obj = spellPrefab; 
            break;
            default: 
                    if(s.cluster)
                        obj = shatter;
                    else
                        obj = spellPrefab;
                    break;
        }

        return obj;
    }
}
