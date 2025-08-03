using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUntilReset : MonoBehaviour
{
    public float moveSpeed = 5;
    public Transform objToTurnOn;

    private Vector3 startPos;
    private Quaternion startRot;

    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider trig)
    {
        if (objToTurnOn) objToTurnOn.gameObject.SetActive(true);
        transform.position = startPos;
        transform.rotation = startRot;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (objToTurnOn) objToTurnOn.gameObject.SetActive(true);
        transform.position = startPos;
        transform.rotation = startRot;
        gameObject.SetActive(false);
    }
}
