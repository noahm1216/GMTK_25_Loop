using UnityEngine;

public class CharacterController3D : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;
    public float groundCheckRadius = 0.3f;

    private float verticalVelocity = 0f;
    private bool isGrounded;
    
    public LayerMask groundMask;

    void Update()
    {
        // LEFT/RIGHT movement
        float input = Input.GetAxis("Horizontal");
        Vector3 horizontalMovement = new Vector3(input, 0f, 0f) * speed;

        // GROUND CHECK with SphereCast
        Vector3 origin = transform.position;// + groundCheckOffset;
        isGrounded = Physics.CheckSphere(origin, groundCheckRadius, groundMask);

        //  Gravity
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 verticalMovement = new Vector3(0f, verticalVelocity, 0f);
        Vector3 movement = (horizontalMovement + verticalMovement) * Time.deltaTime;

        transform.Translate(movement, Space.World);
        
    }
}