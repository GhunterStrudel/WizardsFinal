using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Assets.Code
{
    class Graph
    {
        public Dictionary<string, Node> vertexMap = new Dictionary<string, Node>();
        public Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        public Dictionary<Node, int> costSoFar
            = new Dictionary<Node, int>();
        public double INFINITY = Double.MaxValue;
        public int maxIteration = 5;

        public Graph()
        {
        }
        public void addEdge(Vector2 pos, Vector2 posSecond, double cost)
        {
            Node v = getVertex(pos);
            Node w = getVertex(posSecond);
            v.adj.AddFirst(new Edge(cost, w));
            w.adj.AddFirst(new Edge(cost, v));
        }
        public static int Heuristic(Vector2 a, Vector2 b)
        {
            return (int)(System.Math.Abs(a.x - b.x) + System.Math.Abs(a.y - b.y));
        }

        public List<Vector2> GetPath(Vector2 startPos, Vector2 endPos)
        {
            clearAll();
            Node start = null;
            Node end = null;
             //Find closest point
            foreach(var item in vertexMap.Values)
            {
                if (start == null)
                    start = item;
                else
                {
                    if (Graph.Heuristic(startPos, start.pos) > Graph.Heuristic(startPos, item.pos))
                        start = item;
                }
                if (end == null)
                    end = item;
                else
                {
                    if (Graph.Heuristic(endPos, end.pos) > Graph.Heuristic(endPos, item.pos))
                        end = item;
                }
            }
            //Debug.Log(vertexMap.Count);

            var pq = new PriorityQueue();
            pq.Enqueue(start, 0);

            cameFrom[start] = start;
            costSoFar[start] = 0;
            int count = 0;
            while (pq.Count > 0)
            {
                var current = pq.Dequeue();

                //if (current.Equals(end) || count == maxIteration)
                //{
                //    end = current;
                //    break;
                //}
                if (current.Equals(end))
                {
                    break;
                }
                count++;
                foreach (var next in current.adj)
                {
                    int newCost = costSoFar[current] + (int)next.Cost;
                    if (!costSoFar.ContainsKey(next.Dest)
                        || newCost < costSoFar[next.Dest])
                    {
                        costSoFar[next.Dest] = newCost;
                        int priority = newCost + Heuristic(next.Dest.pos, end.pos);
                        pq.Enqueue(next.Dest, priority);
                        cameFrom[next.Dest] = current;
                        next.Dest.prev = current;
                    }
                }
            }
            //foreach (var key in cameFrom.Keys)
            //    key.scratch = 1;    
            //foreach (var item in cameFrom.Values)
            //    item.scratch = 1;
            List<Vector2> sol = new List<Vector2>();       
            sol.Add(endPos);
            Node temp = end;
            while(temp.prev != null)
            {
                sol.Add(temp.pos);
                temp = temp.prev;
                temp.scratch = 1;
            }
            sol.Reverse();
            return sol;
        }
        public Node getVertex(Vector2 pos)
        {
            Node v;
            if (vertexMap.TryGetValue(pos.ToString(), out v))
            {
                int i = 0;
            }
            else
            {
                v = new Node(pos);
                vertexMap.Add(pos.ToString(), v);
            }
            return v;
        }
        public void clearAll()
        {
            cameFrom = new Dictionary<Node, Node>();
            costSoFar = new Dictionary<Node, int>();
            foreach (var item in vertexMap.Values)
            {
                item.reset();
            }
        }
        public void clearDraw()
        {
            foreach (var item in vertexMap.Values)
            {
                item.isDrawn = 0;
            }
        }
    }
}
