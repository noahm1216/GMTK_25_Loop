using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    public Animator animCon;
    public Rigidbody rb3D;


    // Start is called before the first frame update
    void Start()
    {
        if (!animCon || !rb3D) return;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!animCon || !rb3D) return;

        animCon.SetBool("running", rb3D.velocity != Vector3.zero);
        animCon.SetBool("right", rb3D.velocity.x > 0);

        if (rb3D.velocity != Vector3.zero) animCon.speed = Mathf.Abs(rb3D.velocity.x * 0.1f); 
        else animCon.speed = 1;
    }
}
