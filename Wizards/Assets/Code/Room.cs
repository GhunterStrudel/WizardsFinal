using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    //script for a room
    class Room : MonoBehaviour
    {
        public bool isVisisted = false;
        public float leftBound = -10f;
        public float rightBound = 10f;
        public float upperBound = 100f;
        public float lowerBound = -100f;
        public int musicID;

        private Manager manager;

        void Start()
        {
            //manager = new Manager();
        }

    }
}
