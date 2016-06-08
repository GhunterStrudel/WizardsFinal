using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller3D))]
public class PlayerController : MonoBehaviour
{

    public float jumpHeight = 4;
    //How long does it take to reach the jump height in seconds.
    public float timeToJumpApex = .4f;
    //acceleration is used to create a more realistic moving effect
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    Cast cast;

    Controller3D controller;
    public GameObject playerSprite;
    float startScale;
    public float oldInput = 1;

    void Start()
    {
        controller = GetComponent<Controller3D>();
        cast = GetComponent<Cast>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        startScale = playerSprite.transform.localScale.x;
    }

    void Update()
    {

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        //Input is used to get the pressed controls and turn them into a vector2.
        Vector2 input = Vector2.zero;

        if(!cast.casting)
            if(WiimoteStatus.WiiEnabled)
                input = new Vector2(WiimoteStatus.nunchuckX/90, WiimoteStatus.nunchuckY/90);
            else
                input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if(Input.GetKeyDown(KeyCode.LeftShift))
                {
                    moveSpeed = 60;
                    jumpHeight = 40;
                }

                if(Input.GetKeyUp(KeyCode.LeftShift))
                {
                    moveSpeed = 6;
                    jumpHeight = 4;
                }

        if(oldInput != input.x && input.x != 0)
            oldInput = input.x;

        playerSprite.transform.localScale = new Vector3(startScale*oldInput, playerSprite.transform.localScale.y, playerSprite.transform.localScale.z);


        if(WiimoteStatus.WiiEnabled)
        {
            if (WiimoteStatus.buttonZ && controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }
        }
        

        float targetVelocityX = input.x * moveSpeed;
        //smooth the acceleration for creating a more realistic movement.
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}