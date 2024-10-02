using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float walkSpeed = 8f;
    public float sprintSpeed = 14f;
    public float maxVelocityChange = 10f;

    [Header("Jump")] [Range(0,1f)]
    public float airControl = 0.5f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float groundCheckDistance = 0.75f;

    Vector2 input;
    Rigidbody rb;
    bool sprinting;
    bool jumping;
    bool grounded;
    Vector3 lastTargetVelocity;
   
    void Start() => rb = GetComponent<Rigidbody>();

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();

        sprinting = Input.GetKey(KeyCode.LeftShift);
        jumping = Input.GetKey(KeyCode.Space);
    }


    void OnCollisionStay(Collision other)
    {
        grounded = true;
    }

    void FixedUpdate()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
        if (grounded)
        {
            if (jumping)
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            else
                ApplyMovement(sprinting ? sprintSpeed : walkSpeed, false);
        }
        else
        {
            if (input.magnitude > 0.5f)
                ApplyMovement(sprinting ? sprintSpeed : walkSpeed, true);
        }
        grounded = false;
    }

    void ApplyMovement(float _speed, bool _inAir)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity) * _speed;

        if (_inAir)
            targetVelocity += lastTargetVelocity * (1 - airControl);

        Vector3 velocityChange = targetVelocity - rb.velocity;

        if (_inAir)
        {
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange * airControl,
                maxVelocityChange * airControl);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange * airControl,
                maxVelocityChange * airControl);
        }
        else
        {
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange,
                maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange,
                maxVelocityChange);
        }

        velocityChange.y = 0;
        
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
}
