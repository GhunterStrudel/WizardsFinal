using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
    class Seek : Enemy
    {
        public GameObject target;
        private float Range;
        public float maxAcceleration = 10f;

        private Rigidbody rb;

        public override void Start()
        {
            base.Start();
            rb = GetComponent<Rigidbody>();
        }

        public override void Update()
        {
            base.Update();

            Vector3 accel = GetAccel(target.transform.position, maxAcceleration);

            steer(accel);

            transform.LookAt(target.transform);

        }

        public void steer(Vector3 linearAcceleration)
        {
            rb.velocity += linearAcceleration * Time.deltaTime;

            if (rb.velocity.magnitude > maxAcceleration)
            {
                rb.velocity = rb.velocity.normalized * maxAcceleration;
            }
        }

        public Vector3 GetAccel(Vector3 targetPosition, float maxSeekAccel)
        {
            //Get the direction
            Vector3 acceleration = targetPosition - transform.position;

            //Remove the z coordinate
            acceleration.z = 0;

            acceleration.Normalize();

            //Accelerate to the target
            acceleration *= maxSeekAccel;

            return acceleration;
        }

    }
}