using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
    class PuzzleManager : Manager
    {

        void Update()
        {
            if(!isDone)
                IsSolved();
        }
        public override void Reset()
        {
            foreach(var item in elements)
            {
                Switch sw = item.GetComponent<Switch>();
                if(sw != null)
                    sw.isHit = false;
            }
            base.Reset();
        }
        public override bool IsSolved()
        {
            foreach(var item in elements)
            {
                Switch sw = item.GetComponent<Switch>();
                if (!sw.isHit)
                    return false;
            }
            return base.IsSolved();
        }
    }
}
