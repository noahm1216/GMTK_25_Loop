using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Transform playerModel; // we'll scale the X based on our intended direction
    public Animator animCon;
    public Rigidbody rb3D;

    private Quaternion startingModelRotation;


    // Start is called before the first frame update
    void Start()
    {
        if (!animCon || !rb3D || !playerModel) return;

        startingModelRotation = playerModel.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!animCon || !rb3D || !playerModel) return;

        animCon.SetBool("running", rb3D.velocity != Vector3.zero); // moving left/right = running
        animCon.SetBool("falling", rb3D.velocity.y <= -0.15f); // going down = falling
        //animCon.SetBool("right", rb3D.velocity.x > 0); // checking direction for left or right

        if (rb3D.velocity.x >= 0) playerModel.rotation = startingModelRotation; // rotate our model based on the direction we're running
        else { playerModel.rotation = startingModelRotation; playerModel.Rotate(0, 55, 0); }

        if (rb3D.velocity != Vector3.zero) animCon.speed = Mathf.Abs(rb3D.velocity.x * 0.1f); // set animation speed for running based on velocity 
        else animCon.speed = 1.5f;
    }

    public void TriggerJump()
    {
        ResetTriggers();
        animCon.SetTrigger("jumped");
    }

    public void ResetTriggers()
    {
        animCon.ResetTrigger("jumped");

    }
}
