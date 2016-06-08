using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code
{
    class GraphGenerator : MonoBehaviour
    {
        private Room room;
        public float leftBound, rightBound, upperBound, lowerBound;
        private Vector3 globalWaypointPos;
        public Graph graph;
        public Vector3 startNode;
        public float radius = 4f;
        //Density
        public float width = 40; //x columns
        public float height = 40; //y rows
        private float widthBetweenNode;
        private float heightBetweenNode;
        private LayerMask layerMask;
        void Awake()
        {
            layerMask = ~((1 << 2) | (1 << 11) | (1 << 10) | (1 << 12));
           // Debug.Log("Generate graph");
            graph = new Graph();
            globalWaypointPos = startNode + transform.position;
            widthBetweenNode = (rightBound - leftBound) / width;
            heightBetweenNode = (upperBound - lowerBound) / height;
           // Debug.Log("width: " + width + " heigh:" + height);
           // Debug.Log("width: " + widthBetweenNode + " heigh:" + heightBetweenNode);
            FloodFill(globalWaypointPos.x, globalWaypointPos.y);
        }
    

        private bool checkCollision(Vector2 position)
        {

            //if (Physics.CheckSphere(new Vector3(position.x, position.y, 0), radius, layerMask))
            //{
            //    return true;
            //} 
            if (Physics.CheckBox(new Vector3(position.x, position.y, 5.9f), new Vector3(0.25f, 0.3f, 0.5f), Quaternion.identity, layerMask))
                return true;
            else
                return false;
        }
        private void FloodFill(float x, float y)
        {
            //Debug.Log("X:" + x +" Y:" + y);
           // Debug.Log("right: " + rightBound + " left: " + leftBound + "upper: " + upperBound + " lower: " + lowerBound);
            for(float i = x; i <= rightBound; i += widthBetweenNode)
            {
                for (float j = y; j <= upperBound; j += heightBetweenNode)
                {
                    Vector2 pos = new Vector2(i, j);
                    if (checkCollision(new Vector3(i, j, 0)))
                    {
                        continue;
                    }
                    if (i < leftBound || i > rightBound || j > upperBound || j < lowerBound)
                        continue;

                    //Check for collision
                    Vector2 above = new Vector2(i, j + heightBetweenNode);
                    if (!checkCollision(above))
                        graph.addEdge(pos, above, 1);
                    Vector2 rightUpper = new Vector2(i + widthBetweenNode, j + heightBetweenNode);
                    if (!checkCollision(rightUpper))
                            graph.addEdge(pos, rightUpper, 1);
                    Vector2 rightUnder = new Vector2(i + widthBetweenNode, j - heightBetweenNode);
                    if (!checkCollision(rightUnder))
                        graph.addEdge(pos, rightUnder, 1);
                    Vector2 right = new Vector2(i + widthBetweenNode, j);
                    if (!checkCollision(right))
                        graph.addEdge(pos, right, 1);
                }
            }
            if (x < leftBound || x > rightBound || y > upperBound || y < lowerBound)
                return;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            float size = .3f;
            globalWaypointPos = startNode + transform.position;
            Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
            Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            if (graph != null)
            {
                foreach (var item in graph.vertexMap.Values)
                {
                    //Gizmos.DrawLine(new Vector3(item.pos.x, item.pos.y, 5.9f) - Vector3.up * size, new Vector3(item.pos.x, item.pos.y, 5.9f) + Vector3.up * size);
                    //Gizmos.DrawLine(new Vector3(item.pos.x, item.pos.y, 5.9f) - Vector3.left * size, new Vector3(item.pos.x, item.pos.y, 5.9f) + Vector3.left * size);
                    foreach (var edge in item.adj)
                    {
                        if (item.scratch == 1 && edge.Dest.scratch == 1)
                            Gizmos.color = Color.yellow;
                        else
                            Gizmos.color = Color.red;
                        Gizmos.DrawLine(new Vector3(item.pos.x, item.pos.y, 5.9f), new Vector3(edge.Dest.pos.x, edge.Dest.pos.y, 5.9f));
                    }
                }
            }
        }

        public List<Vector2> GetPath(Vector2 startPos, Vector2 endPos)
        {
            return graph.GetPath(startPos, endPos);
        }

    }
}
