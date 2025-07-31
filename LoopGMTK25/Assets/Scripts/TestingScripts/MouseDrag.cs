using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseDrag : MonoBehaviour
{
    
    public LayerMask moveableLayers;

    private Camera camMain;
    private Transform gearOnHand;
    private Rigidbody rbodyOnHand;
    private bool runRaycast;

    public TextMeshProUGUI textFieldDebug;


    private void OnEnable()
    {
        if (!camMain) camMain = Camera.main;
    }

    private void FixedUpdate()
    {
        if (runRaycast)
        {
            print("Raycasting");

            runRaycast = false;

            if (!camMain) Debug.LogWarning("Missing Main Camera");
            Ray ray = camMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, moveableLayers))
            {
                print("Raycast Hit");
                if (!gearOnHand) { gearOnHand = hit.transform; gearOnHand.TryGetComponent(out rbodyOnHand); }
                if (rbodyOnHand) { rbodyOnHand.isKinematic = true; rbodyOnHand.angularVelocity = Vector3.zero; } // stop the gear from spinning and remove its future physics impact
            }
            else
                print("Raycast not hit");
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) runRaycast = true;

        if (gearOnHand)
        {
            print("Holding The Gear");
            Vector3 mouseWorldPos = camMain.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camMain.transform.position.z));
            //mouseWorldPos.z = gearOnHand.position.z;
            gearOnHand.transform.position = mouseWorldPos;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && gearOnHand)
        {
            if (rbodyOnHand) { rbodyOnHand.isKinematic = false; }
            gearOnHand = null;
        }

        if (textFieldDebug && camMain)
        {
            Vector3 mouseWorldPos = camMain.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camMain.transform.position.z));
            textFieldDebug.text = mouseWorldPos.ToString();
        }
    }


}
