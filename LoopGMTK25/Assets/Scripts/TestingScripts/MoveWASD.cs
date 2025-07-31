using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWASD : MonoBehaviour
{
    public Transform objectToMove;
    public float baseSpeed = 10;
    public float speedMultiplier = 1;

    private float storedSpeedMultiplier;

    private Vector3 holdDirection;

    private void OnEnable()
    {
        storedSpeedMultiplier = speedMultiplier;
        if (!objectToMove) objectToMove = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) MoveObj(new Vector3(-baseSpeed, 0, 0)); // left
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) MoveObj(new Vector3(baseSpeed, 0, 0)); // right
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) MoveObj(new Vector3(0, baseSpeed, 0)); // up
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) MoveObj(new Vector3(0, -baseSpeed, 0)); // down


        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) speedMultiplier = storedSpeedMultiplier * 2; // speedUp
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) speedMultiplier = storedSpeedMultiplier; // stop speedup

        if(holdDirection != Vector3.zero) MoveObj(holdDirection); // move hold while pressing UI buttons
        if (Input.GetKeyDown(KeyCode.Mouse0)) holdDirection = Vector3.zero;


    }

    public void MoveUp(bool _up)
    {
        if (_up) holdDirection = new Vector3(0, baseSpeed, 0);  // up
        else holdDirection = new Vector3(0, -baseSpeed, 0); // down
    }

    public void MoveRight(bool _right)
    {
        if (_right) holdDirection = new Vector3(baseSpeed, 0, 0); // right
        else holdDirection = new Vector3(-baseSpeed, 0, 0); // left
    }


    public void MoveObj(Vector3 _direction)
    {
        if (!objectToMove) return;

        objectToMove.Translate(_direction * speedMultiplier * Time.deltaTime);
    }
}
