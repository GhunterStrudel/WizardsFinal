using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public enum Direction { Left, Right, Up, Down }
    class Enter : MonoBehaviour
    {
        public Direction direction;
        public GameObject GetRoom()
        {
            return this.transform.parent.gameObject;
        }
    }
}
