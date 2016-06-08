using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
    public class Switch : MonoBehaviour
    {
        public bool isHit { get; set; }
        public ElementType weakness;
        void Start()
        {
            isHit = false;
        }
        void OnCollisionEnter(Collision col)
        {
            if (col.collider.tag == "Spell" || col.collider.tag == "Cluster")
            {
                CastSpell c = col.gameObject.GetComponent<CastSpell>();

                if(c != null)
                {
                    if(c.elementType == weakness)
                    {
                        isHit = true;
                    }
                }
                else if(col.gameObject.GetComponent<ShatterShells>().elementType == weakness)
                    isHit = true;
            }
            else if (col.collider.tag == "Pillar")
            {
                if (col.gameObject.transform.parent.GetComponent<CastSpell>().elementType == weakness)
                {
                    isHit = true;
                }
            }
        }

    }
}
