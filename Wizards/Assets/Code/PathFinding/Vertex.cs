using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    class Vertex
    {
        public string Name { get; set; }
        public LinkedList<Edge> adj { get; set; }
        public double dist { get; set; }
        public Vertex prev { get; set; }
        public Vertex next { get; set; }
        public int scratch { get; set; }
        public Vertex(string nm)
        {
            Name = nm;
            adj = new LinkedList<Edge>();
            reset();
        }
        public void reset()
        {
            dist = Double.MaxValue;
            prev = null;
            next = null;
            scratch = 0;
        }
    }
}
