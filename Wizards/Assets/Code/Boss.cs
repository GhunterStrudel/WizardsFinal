using UnityEngine;
using System.Collections;
using System;

public class Boss : Enemy
{
    bool playerInRange = false;
    CastSpell cs;

    private Vector3 playerLocation;
    private Transform player;
    private float playerHeight;
    public int spellID = 0;

    public float elementTime = 10f;
    float elementTimer;
    public float castTime = 3f;
    float castTimer;

    public SpellBook sb;
    public GameObject spellPrefab;
    public GameObject stonePillar;
    public GameObject cloud;
    public GameObject cannon;
    public GameObject channel;
    public PhysicMaterial bounce;
    public ParticleSystem castEffect;

    StateMachine sm;

    // Use this for initialization
    public override void Start () {
        castTimer = castTime;
        elementTimer = elementTime;
        sm = new StateMachine(elementType);
        castEffect.startColor = sb.spells[spellID].baseColor;
        castEffect.Play();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        if (playerInRange)
        {
            if (castTimer <= 0f)
            {
                CastSpell();
                castTimer = castTime;
            }
        }
        castTimer -= Time.deltaTime;

        if(elementTimer < 0f)
        {
            ChangeElement();
            elementTimer = elementTime;
            castTimer = castTime;
        }
        elementTimer -= Time.deltaTime;
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == "Player")
        {
            playerLocation = other.transform.position;
            player = other.transform;
            playerInRange = true;
            playerHeight = other.GetComponent<MeshFilter>().mesh.bounds.extents.x;
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if (other.tag == "Player")
        {
            playerLocation = other.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void ChangeElement()
    {
        StateMachine.Command comm;
        if (health < (startingHealth / 4)) comm = StateMachine.Command.HealthLow;
        else if (health < (startingHealth / 2)) comm = StateMachine.Command.HealthMediumLow;
        else if (health < ((startingHealth / 4) * 3)) comm = StateMachine.Command.HealthMediumHigh;
        else comm = StateMachine.Command.HealthHigh;

        elementType = sm.MoveNext(comm);

        switch (elementType)
        {
            case ElementType.Fire:  spellID = 0;    castEffect.startColor = sb.spells[spellID].baseColor; break;
            case ElementType.Air:   spellID = 3;    castEffect.startColor = sb.spells[spellID].baseColor; break;
            case ElementType.Ice:   spellID = 6;    castEffect.startColor = sb.spells[spellID].baseColor; break;
            case ElementType.Earth: spellID = 9;    castEffect.startColor = sb.spells[spellID].baseColor; break;
            case ElementType.Dark:  spellID = 12;   castEffect.startColor = sb.spells[spellID].baseColor; break;
            case ElementType.Light: spellID = 15;   castEffect.startColor = sb.spells[spellID].baseColor; break;
            default:                spellID = 0;    castEffect.startColor = sb.spells[spellID].baseColor; break;
        }
        if (health < (startingHealth / 3) && elementType != ElementType.Dark && elementType != ElementType.Light)
            spellID++;
    }

    void CastSpell()
    {
        Spell spell = sb.spells[spellID];

        GameObject s;

        switch (spell.castType)
        {
            case CastType.Uproot: s = Instantiate(stonePillar, gameObject.transform.position, Quaternion.identity) as GameObject; break;
            case CastType.Summon: if (spell.elementType == ElementType.Air) s = Instantiate(cloud, gameObject.transform.position, Quaternion.identity) as GameObject;
                else s = Instantiate(stonePillar, gameObject.transform.position, Quaternion.identity) as GameObject; break;
            case CastType.Channel: s = Instantiate(channel, gameObject.transform.position, Quaternion.identity) as GameObject; break;
            default: s = Instantiate(spellPrefab, gameObject.transform.position, Quaternion.identity) as GameObject; break;
        }

        cs = s.GetComponent<CastSpell>();

        cs.damage = spell.damage;
        cs.name = spell.name;
        cs.elementType = spell.elementType;
        cs.castType = spell.castType;
        cs.damageType = spell.damageType;
        cs.dotTimer = spell.dotTimer;
        cs.aoe = spell.aoe;
        cs.aoeRange = spell.aoeRange;
        cs.bouncing = spell.bouncing;
        cs.baseColor = spell.baseColor;
        cs.effectTexture = spell.effectTexture;
        cs.lifeTime = spell.lifeTime;
        cs.castType = spell.castType;
        cs.damageType = spell.damageType;
        cs.dotTimer = spell.dotTimer;
        cs.aoe = spell.aoe;
        cs.aoeRange = spell.aoeRange;
        cs.bouncing = spell.bouncing;
        cs.cluster = spell.cluster;
        cs.baseColor = spell.baseColor;
        cs.effectTexture = spell.effectTexture;
        
        switch (cs.castType)
        {
            case CastType.Direction: CastDirection(s); break;
            case CastType.Target: CastTarget(s); break;
            case CastType.Uproot: CastTarget(s); break;
            case CastType.Channel: CastChannel(s); break;
            case CastType.Summon: CastSummon(s); break;
        }
    }

    void CastDirection(GameObject spell)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(spell.transform.position);
        Vector3 dir;
        spell.SetActiveRecursively(true);

        if (cs.bouncing)
        {
            spell.GetComponent<Collider>().material = bounce;
            spell.GetComponent<Rigidbody>().useGravity = true;
        }

        Physics.IgnoreCollision(spell.GetComponent<Collider>(), GetComponent<Collider>());

        dir = Camera.main.WorldToScreenPoint(playerLocation) - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        spell.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void CastTarget(GameObject spell)
    {
        switch (cs.elementType)
        {
            case ElementType.Light: cs.targetObj = gameObject.transform; break;
            case ElementType.Earth: cs.target = new Vector3(playerLocation.x + ((gameObject.transform.position.x - playerLocation.x) / 2), playerLocation.y, playerLocation.z); break;
            case ElementType.Dark: cs.targetObj = player; break;
        }        

        spell.SetActiveRecursively(true);
    }
    
    public void CastChannel(GameObject spell)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(spell.transform.position);
        Vector3 dir;
        Physics.IgnoreCollision(spell.GetComponent<Collider>(), GetComponent<Collider>());
        spell.SetActiveRecursively(true);
        
        dir = Camera.main.WorldToScreenPoint(playerLocation) - pos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        spell.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void CastSummon(GameObject spell)
    {
        switch (cs.elementType)
        {
            case ElementType.Air: spell.transform.localPosition = new Vector3(playerLocation.x, playerLocation.y + 2, 5.9f); break;
            case ElementType.Earth: spell.transform.localPosition = new Vector3(playerLocation.x + ((gameObject.transform.position.x - playerLocation.x) / 2), playerLocation.y + 2, playerLocation.z); break;
        }
        spell.SetActiveRecursively(true);
    }
}
