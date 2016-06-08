using UnityEngine;
using System.Collections;

public class Cast : MonoBehaviour {

    GameObject spell;
    CastSpell cs;
    public bool casting = false;
    public PhysicMaterial bounce;

    public GameObject pointer;
    //public GameObject wiiPointer;
    public RectTransform wiiPointer;
    public ParticleSystem castEffect;
    bool fireSpell;

    public int summonSpellLimit;
    GameObject[] summonList;

    void Start()
    {
        summonList = new GameObject[summonSpellLimit];
    }

    // Update is called once per frame
    void Update ()
    {

        if (casting)
        {
            switch (cs.castType)
            {
                case CastType.Direction: CastDirection(); break;
                case CastType.Target: CastTarget(); break;
                case CastType.Uproot : CastTarget(); break;
                case CastType.Channel: CastChannel(); break;
                case CastType.Summon: CastSummon(); break;
            }

            castEffect.Play();

            if(Input.GetMouseButtonDown(1))
            {
                Cancel();
            }
        }
        else
        {
            if(castEffect.IsAlive())
                castEffect.Stop();
        }
    }

    public void Cancel()
    {
        Destroy(spell.gameObject);
        cs = null;
        casting = false;
    }

    public void CastAble(GameObject s)
    {
        spell = s;
        cs = s.GetComponent<CastSpell>();
        casting = true;
        cs.caster = gameObject.GetComponent<Cast>();
    }

    public void CastDirection()
    {
        
        Vector3 pos = Camera.main.WorldToScreenPoint(pointer.transform.position);
        Vector3 dir;

        if(WiimoteStatus.WiiEnabled)
            dir = wiiPointer.transform.position - pos;
        else
            dir = Input.mousePosition - pos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if(WiimoteStatus.WiiEnabled)
            fireSpell = WiimoteStatus.buttonA;
        else
            fireSpell = Input.GetMouseButton(0);

        if (fireSpell)
        {
            spell.transform.position = transform.position;
            spell.SetActiveRecursively(true);

            if(cs.bouncing)
            {
                spell.GetComponent<Collider>().material = bounce;
                spell.GetComponent<Rigidbody>().useGravity = true; 
            }

            Physics.IgnoreCollision(spell.GetComponent<Collider>(), GetComponent<Collider>());
            spell.transform.rotation = pointer.transform.rotation;
            casting = false;
            //Debug.Break();
        }

    }

    public void CastTarget()
    {
        RaycastHit hit;

        if(WiimoteStatus.WiiEnabled)
        {
            if(WiimoteStatus.buttonA)
            {
                Ray ray = Camera.main.ScreenPointToRay(wiiPointer.transform.position);

                if (Physics.Raycast(ray, out hit, 100))
                    if(hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Player")
                        TargetCast(hit);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out hit, 100))
                    if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Player")
                        TargetCast(hit);
                
            }
        }
    }

    bool channeling;
    public void CastChannel()
    {

        Vector3 pos = Camera.main.WorldToScreenPoint(spell.transform.position);
        Vector3 dir; 
        Physics.IgnoreCollision(spell.GetComponent<Collider>(), GetComponent<Collider>());     

        if(WiimoteStatus.WiiEnabled)
            fireSpell = WiimoteStatus.buttonA;
        else
            fireSpell = Input.GetMouseButton(0);


        if (fireSpell)
        {
            channeling = true;
            spell.SetActiveRecursively(true);

                    if(WiimoteStatus.WiiEnabled)
            dir = wiiPointer.transform.position - pos;
        else
            dir = Input.mousePosition - pos;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            spell.transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        }
        else if(!fireSpell && channeling)
        {
            casting = false;
            channeling = false;
            cs.StopEmit();
        }

    }

    int summonCount;
    public void CastSummon()
    {
        Vector3 mousePosition = Vector3.zero;

        if(WiimoteStatus.WiiEnabled)
        {
           mousePosition = new Vector3(wiiPointer.transform.position.x, wiiPointer.transform.position.y, 5.9f);
           fireSpell = WiimoteStatus.buttonA;
        }
        else
        {
            mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5.9f);
            fireSpell = Input.GetMouseButton(0);
        }

        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if(fireSpell)
        {
            summonCount++;

            if(summonCount==summonSpellLimit)
                summonCount = 0;
            
            if(summonList[summonCount] != null)
                Destroy(summonList[summonCount].gameObject);

            spell.transform.localPosition = new Vector3(objPosition.x, objPosition.y, 5.9f);

            if((cs.elementType == ElementType.Dark||cs.elementType == ElementType.Earth) && Vector3.Dot(Vector3.right, objPosition - transform.position) < 0)
            {
                if (cs.elementType == ElementType.Earth)
                    cs.GetComponent<Cannon>().rot -= 180;
                else {

                    cs.GetComponent<VoidPortal>().rot = 180;
                    cs.GetComponent<VoidPortal>().pRot = 135;
                }
            } 
            spell.SetActiveRecursively(true);

            summonList[summonCount] = spell;

            casting = false;
        }

    }

    public void TargetCast(RaycastHit hit)
    {
        if(cs.castType == CastType.Uproot)
        {
            cs.target = new Vector3(hit.point.x, hit.collider.gameObject.transform.position.y + (hit.collider.gameObject.transform.localScale.y/2.1f), 5.9f);
            cs.targetRotation = hit.transform.rotation;
            Debug.Log(hit.point);
        }
        else
            cs.targetObj = hit.collider.gameObject.transform;

        spell.SetActiveRecursively(true);
        casting = false;
    }
}
