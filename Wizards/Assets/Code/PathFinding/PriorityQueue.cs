using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Assets.Code
{
    //ONLT FOR INTS YO
    public class PriorityQueue
    {
        //private List<Tuple<Node, int>> elements = new List<Tuple<Node, int>>();
        private Dictionary<Node, int> elements = new Dictionary<Node, int>();

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(Node item, int priority)
        {
            elements.Add(item, priority);
        }

        public Node Dequeue()
        {
            int best = int.MinValue;
            Node current = null;
            foreach(var item in elements)
            {
                if (best == int.MinValue)
                {
                    best = item.Value;
                    current = item.Key;
                }
                else if (item.Value < best)
                {
                    best = item.Value;
                    current = item.Key;
                }
            }
            if(current != null)
                elements.Remove(current);
            return current;
        }
    }
}
