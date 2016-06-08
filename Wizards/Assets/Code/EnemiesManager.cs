using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
    class EnemiesManager : Manager
    {
        void Awake()
        {
            foreach (var item in elements)
            {
                Enemy enemy = item.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Awake();
                }
            }
        }
        void OnEnable()
        {
            foreach (var item in elements)
            {
                Enemy enemy = item.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Reset();
                }
            }
            Reset();
        }
        void Update()
        {
            if (!isDone)
                IsSolved();
        }
        public override bool IsSolved()
        {
            foreach (var item in elements)
            {
                Enemy enemy = item.GetComponent<Enemy>();
                if (enemy != null)
                    if(enemy.health > 0)
                        return false;
            }
            return base.IsSolved();
        }
    }
}
