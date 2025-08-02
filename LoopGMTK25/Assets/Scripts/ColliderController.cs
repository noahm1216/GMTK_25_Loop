using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public Bounds visualBounds;
    public Bounds colliderBounds;
    private MeshRenderer _meshRenderer;
    public BoxCollider boxCollider;
    
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        visualBounds = _meshRenderer.bounds;
        colliderBounds = boxCollider.bounds;
    }
}
