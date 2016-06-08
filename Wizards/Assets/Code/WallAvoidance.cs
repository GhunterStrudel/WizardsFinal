using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
    class WallAvoidance : Enemy
    {
        public GameObject target;
        public GraphGenerator graph;

        private List<Vector2> path;
        private int currentWayPoint = 0;
        private Vector3 targetPosition = Vector3.zero;
        public float interval = 30;
        private float timeLeft;
        private Rigidbody rb;

        void Awawe()
        {
            base.Awake();
            timeLeft = interval;
        }
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        void Update()
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                //Debug.Log("new path");
                path = graph.GetPath(transform.position, target.transform.position);
                //Debug.Log(path.Count);
                //foreach (var item in path)
                //{
                //    Debug.Log(item);
                //}
                timeLeft = interval;
                targetPosition = Vector3.zero;
                currentWayPoint = 0;
            }

            if (currentWayPoint < this.path.Count)
            {
                if (targetPosition == Vector3.zero)
                    targetPosition = path[currentWayPoint];
                Debug.Log("Walk");
                walk();
            }
        }
        void walk()
        {
            // rotate towards the target

            // move towards the target
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, targetPosition.y, 5.9f), speed * Time.deltaTime);
            //transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(targetPosition.x, targetPosition.y, 5.9f) - transform.position, speed * Time.deltaTime, 0.0f);

            //transform.position = new Vector3(targetPosition.x, targetPosition.y, 5.9f);
            if (rb != null)
                Debug.Log("Velocity: " + rb.velocity);
            Debug.Log(targetPosition);
            if (transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)
            {
                Debug.Log("next position");
                currentWayPoint++;
                targetPosition = path[currentWayPoint];
            }
        }
    }
}
