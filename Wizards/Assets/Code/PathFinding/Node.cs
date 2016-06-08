using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class Node
    {
        public Vector2 pos { get; set; }
        public LinkedList<Edge> adj { get; set; }
        public double dist { get; set; }
        public Node prev { get; set; }
        public Node next { get; set; }
        public int scratch { get; set; }
        public int isDrawn { get; set; }
        public Node(Vector2 pos)
        {
            this.pos = pos;
            adj = new LinkedList<Edge>();
            reset();
        }
        public void reset()
        {
            dist = Double.MaxValue;
            prev = null;
            next = null;
            scratch = 0;
            isDrawn = 0;
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Node n = (Node)obj;
            if (this == n)
                return 0;
            if ((this.pos.x > n.pos.x))
                return 1;
            else if (this.pos.y > n.pos.y)
                return 1;
            else
                return -1;
        }
    }
}
