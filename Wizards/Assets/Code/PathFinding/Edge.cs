using UnityEngine;

namespace Assets.Code
{
    public class Edge
    {
        public Node Dest { get; set; }
        public double Cost { get; set; }
        public Edge(double cost, Node dest)
        {
            Dest = dest;
            Cost = cost;
        }
    }
}
