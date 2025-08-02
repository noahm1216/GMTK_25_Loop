using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    
    [Header("Move Ability\n______________")]
    public KeyCode key_MoveRight = KeyCode.D;
    public KeyCode key_MoveLeft = KeyCode.A;
    public float maximumMovePower = 5;
    public float moveAccelleration = 0.15f;
    [Range(0.1f, 1)]
    public float changeSpeedOnDirectionChange = 0.5f;

    private float currentMovePower;
    private bool holdingRight, holdingLeft;


    [Space]
    [Header("Jump Ability\n______________")]
    public KeyCode key_Jump = KeyCode.Space;   
    public float maximumJumpPower = 20; // 20 if standardized || 95 if not
    public Rigidbody rb3D;    
    [Range(0, 10)]
    public int numberOfJumps = 2;
    public LayerMask layersThatResetJumps;
   
    [Range(0.00f, 100)]
    public float fallAcceleration = 15f;

    public UnityEvent onPress_Jump;

    private float inputJumpTime; // the time we press down any of the keys
    private bool pressedJump;
    private int timesJumpedSinceLastGround;

    
    private void Start()
    {
        if (!rb3D) TryGetComponent(out rb3D);
    }

    public bool CanJump()
    {
        return timesJumpedSinceLastGround < numberOfJumps;
    }

    private void Update()
    {
        CheckForInputs();
    }

    private void FixedUpdate()
    {
        if (pressedJump) // jumping
        {
            if (rb3D) rb3D.AddForce(((Vector3.up) * maximumJumpPower - rb3D.velocity), ForceMode.VelocityChange);
            timesJumpedSinceLastGround++;
            pressedJump = false;
        }
        // faling / moving down
        if (rb3D.velocity.y < 0) rb3D.AddForce(Vector3.down * (1 * fallAcceleration));

        if (holdingRight)
        {
            currentMovePower += maximumMovePower * moveAccelleration; // steady increase (can change to currentMovePower for exponential
            if (currentMovePower > maximumMovePower) currentMovePower = maximumMovePower;
            if (rb3D) rb3D.AddForce(((Vector3.right) * currentMovePower - rb3D.velocity), ForceMode.Acceleration);
        }
        if (holdingLeft)
        {
            currentMovePower -= maximumMovePower * moveAccelleration; // steady increase (can change to currentMovePower for exponential
            if (currentMovePower < -maximumMovePower) currentMovePower = -maximumMovePower;
            if (rb3D) rb3D.AddForce(((Vector3.right) * currentMovePower - rb3D.velocity), ForceMode.Acceleration);
        }
    }

    private void CheckForInputs()
    {
        if (Input.GetKeyDown(key_Jump) && CanJump())
        {
            onPress_Jump?.Invoke();
            inputJumpTime = Time.time;
            pressedJump = true;
        }

        if (Input.GetKey(key_MoveRight)) // holding right
        {
            if (holdingLeft) currentMovePower *= changeSpeedOnDirectionChange;
            holdingRight = true;
            holdingLeft = false;
        }
        else if (Input.GetKey(key_MoveLeft)) // holding left
        {
            if (holdingRight) currentMovePower *= changeSpeedOnDirectionChange;
            holdingRight = false;
            holdingLeft = true;
        }
        else // not moving left or right
        {
            holdingRight = false;
            holdingLeft = false;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if ((layersThatResetJumps.value & (1 << col.transform.gameObject.layer)) > 0) // collide with object within our specified layers
        {
            timesJumpedSinceLastGround = 0;          
        }      
    }

}
