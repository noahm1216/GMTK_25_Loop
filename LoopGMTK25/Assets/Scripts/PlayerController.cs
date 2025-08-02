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
    public KeyCode key_Up = KeyCode.Space;

    public float maximumJumpPower = 20; // 20 if standardized || 95 if not
    public Rigidbody rb3D;    
    [Range(0, 10)]
    public int numberOfJumps = 2;
    public LayerMask layersThatResetJumps;
   
    [Range(0.00f, 100)]
    public float fallAcceleration = 15f;

    [Range(0.00f, 100)]
    public UnityEvent onPress_Jump;

    //down arrow to increase acceleration of char falling
    public KeyCode key_MoveDown = KeyCode.S;


    private float inputJumpTime, fallMultiplier; // the time we press down any of the keys
    private bool pressedJump, pressedUp;
    private int timesJumpedSinceLastGround;

    [Space]
    [Header("Escape Restart\n______________")]
    //add a vector3 variable global, on start store transform position...
    //during update, press r//esp, reset pos back to start
    public Vector3 originPosition = new Vector3(1, 1, 1);
    public float timeUntilStart = 0.5f;
    public float restartTimestamp;
    private bool pressedEsc;



    private void Start()
    {
        originPosition = transform.position;
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

        if (pressedUp) // jumping--same as spacebar, only for "W" key
        {
            if (rb3D) rb3D.AddForce(((Vector3.up) * maximumJumpPower - rb3D.velocity), ForceMode.VelocityChange);
            timesJumpedSinceLastGround++;
            pressedJump = false;
        }

        // faling / moving down
        if (rb3D.velocity.y < 0) rb3D.AddForce(Vector3.down * (1 * fallAcceleration*fallMultiplier));
       


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
       
        //W button sub for Space
        if (Input.GetKeyDown(key_Up) && CanJump()) {
            onPress_Jump?.Invoke();
            inputJumpTime = Time.time;
            pressedJump = true; 
        }
        
        //S Button for falling speed acceleration
        if (Input.GetKey(key_MoveDown)) fallMultiplier = 3; 
        if (Input.GetKeyUp(key_MoveDown)) fallMultiplier = 1;


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
        if (Input.GetKeyDown(KeyCode.Escape) && pressedEsc == false)
        {
            //gets time after press escape
            restartTimestamp = Time.time;
            pressedEsc = true;
        }
        if ((Time.time > restartTimestamp + timeUntilStart && pressedEsc == true))
        {
            //sets position back to start time
            transform.position = originPosition;
            pressedEsc = false;
            if (rb3D)
            {
                rb3D.velocity = Vector3.zero;
            }
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
