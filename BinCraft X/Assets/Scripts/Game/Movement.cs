using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float accMove;
    public float dccMove;
    public float speedWalkMax;
    public float speedRunMax;
    public float gravity;
    public float speedJump;

    [HideInInspector] public bool active;
    
    [Header("Ref")]
    public CharacterController characterController;
    public Camera camHead;

    private Vector2 velocityMovement;
    private float velocityY;

    void Update()
    {
        if (active)
        {
            // get new movement input
            Vector2 inputMovement = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) { inputMovement += Vector2.up; }
            if (Input.GetKey(KeyCode.S)) { inputMovement += Vector2.down; }
            if (Input.GetKey(KeyCode.A)) { inputMovement += Vector2.left; }
            if (Input.GetKey(KeyCode.D)) { inputMovement += Vector2.right; }

            // decelerate in opposite direction
            if (velocityMovement.x < 0 && inputMovement.x >= 0 || velocityMovement.x > 0 && inputMovement.x <= 0)
            {
                velocityMovement.x = Mathf.MoveTowards(velocityMovement.x, 0, dccMove * Time.deltaTime);
            }
            if (velocityMovement.y < 0 && inputMovement.y >= 0 || velocityMovement.y > 0 && inputMovement.y <= 0)
            {
                velocityMovement.y = Mathf.MoveTowards(velocityMovement.y, 0, dccMove * Time.deltaTime);
            }

            // get move speed
            float speedMove = Input.GetKey(KeyCode.LeftShift) ? speedRunMax : speedWalkMax;

            // update movement velocity
            if (inputMovement.x != 0)
            {
                velocityMovement.x = Mathf.MoveTowards(velocityMovement.x, inputMovement.x * speedMove, accMove * Time.deltaTime);
            }
            if (inputMovement.y != 0)
            {
                velocityMovement.y = Mathf.MoveTowards(velocityMovement.y, inputMovement.y * speedMove, accMove * Time.deltaTime);
            }

            // clamp movement velocity
            velocityMovement = Vector3.ClampMagnitude(velocityMovement, speedMove);

            // calculate movement
            Vector3 movement = Vector3.zero;
            movement += transform.forward * velocityMovement.y;
            movement += transform.right * velocityMovement.x;

            // update gravity
            if (characterController.isGrounded)
            {
                velocityY = 0;
            }
            velocityY -= gravity * Time.deltaTime;

            // jump
            if (Input.GetKey(KeyCode.Space) && characterController.isGrounded)
            {
                velocityY = speedJump;
            }

            // apply movement and gravity
            characterController.Move((movement + Vector3.up * velocityY) * Time.deltaTime);
        }
    }
}
