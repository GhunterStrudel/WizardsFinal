using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    // The target we are following
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    // How much we 
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;


    //camera boundries (standard, will get updatet after u go to do a different room)
    public float leftBound = -10f;
    public float rightBound = 1000;
    public float upperBound = 100f;
    public float lowerBound = -100f;

    // Place the script in the Camera-Control group in the component menu
    [AddComponentMenu("Camera-Control/Smooth Follow")]

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target) return;

        // Calculate the current rotation angles
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the target
        //transform.LookAt(target);
        //Debug.Log(transform.position.x);
        transform.position = new Vector3(
           Mathf.Clamp(transform.position.x, leftBound, rightBound),
           Mathf.Clamp(transform.position.y, lowerBound, upperBound),
           transform.position.z);
    }
    public void changePosition(float left, float right, float upper, float lower)
    {
        leftBound = left;
        rightBound = right;
        upperBound = upper;
        lowerBound = lower;
    }
}