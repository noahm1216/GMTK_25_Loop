using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    public RegionID regionID;
    public LayerMask colliderLayerMask;
    
    public Color regionColor;

    public BoxCollider boxCollider;

    private PlayerController _player;

    private CapsuleCollider playerCollider;

    public enum LayerIndex
    {
        PhysicsA = 6,
        PhysicsB = 7,
        PhysicsC = 8
    }

    public LayerIndex layerIndex;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerCollider = _player.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCollider.bounds.Contains(_player.transform.position))
        {
            playerCollider.excludeLayers = ~((1 << 0) | (1 << (int)layerIndex));
        }
    }

    /*private void OnTriggerEnter(Collider collider)
    {
        GameObject go = collider.gameObject;

        Debug.Log(go.tag);
        if (go.CompareTag("Player"))
        {
            CharacterController3D controller = go.GetComponent<CharacterController3D>();
            controller.groundMask = colliderLayerMask;
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = regionColor;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
    }
}
