using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Code
{
    public class Manager : MonoBehaviour
    {
        [SerializeField]
        protected List<GameObject> elements;
        [SerializeField]
        protected GameObject target;
        //Can be used for the update function
        protected bool isDone;

        void Start()
        {
            Reset();
        }

        public void EnableTarget()
        {
            if(target != null)
                target.SetActive(true);
        }
        public virtual void Reset()
        {
            if (target != null)
                target.SetActive(false);
        }
        public virtual bool IsSolved()
        {
            isDone = true;
            EnableTarget();
            return true;
        }


    }
}
