using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    class WallAvoidanceSeek : Enemy
    {
        public GameObject target;
        // Variables 
        private int direction = -1;
        //public float speed = 20;
        public float rotateSpeed = 10;
        public float maxDistance = 5;
        public float directionDistance = 5;
        public float targetDistance = 5;
        public float followSpeed = 11;
        public System.Random rnd;
        //public Transform Target; 
        // End variables

        void Awake()
        {
            rnd = new System.Random();
        }
        void Update()
        {
            //if (Physics.Raycast(transform.position, transform.right, directionDistance))
            //{
            //    direction = rnd.Next(-1, 2);
            //}
            //// If there is a object at the left side of the object then give a random direction
            //if (Physics.Raycast(transform.position, -transform.right, directionDistance))
            //{
            //    direction = rnd.Next(-1, 2);
            //}
            //// rotate 90 degrees in the random direction
            //transform.Rotate(Vector3.up, 90 * rotateSpeed * Time.smoothDeltaTime * direction);
            transform.LookAt(target.transform);
            transform.Translate(Vector3.forward * followSpeed * Time.smoothDeltaTime);
            // If there is a object at the right side of the object then give a random direction

            //Check Distance between current object and Target
            float dist = Vector3.Distance(target.transform.position, transform.position);
            //print("Distance to other:" +dist);
            // If distance is bigger then distance between target, wander around.
            //if (dist > targetDistance)
            //{
            //    //Check if there is a collider in a certain distance of the object if not then do the following
            //    if (!Physics.Raycast(transform.position, transform.forward, maxDistance))
            //    {
            //        // Move forward
            //        transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime);
            //    }
            //    else
            //    {
            //        // If there is a object at the right side of the object then give a random direction
            //        if (Physics.Raycast(transform.position, transform.right, directionDistance))
            //        {
            //            direction= rnd.Next(-1, 2);
            //        }
            //        // If there is a object at the left side of the object then give a random direction
            //        if (Physics.Raycast(transform.position, -transform.right, directionDistance))
            //        {
            //            direction= rnd.Next(-1, 2);
            //        }
            //        // rotate 90 degrees in the random direction
            //        transform.Rotate(Vector3.up, 90 * rotateSpeed * Time.smoothDeltaTime * direction);
            //    }
            //}
            //// If current distance is smaller than the given ditance, then rotate towards player, and translate the rotation into forward motion times the given speed
            //if (dist < targetDistance)
            //{
            //    transform.LookAt(target.transform);
            //    transform.Translate(Vector3.forward * followSpeed * Time.smoothDeltaTime);
            //}
        }
    }
}
