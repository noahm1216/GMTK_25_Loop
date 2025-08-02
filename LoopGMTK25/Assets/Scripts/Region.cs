using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Region : MonoBehaviour
{
    public LayerMask colliderLayerMask;
    
    public Color regionColor;

    public BoxCollider boxCollider;

    private PlayerController _player;

    private CapsuleCollider playerCollider;

    //private BoxCollider[] _colliders;
    
    private List<ColliderController> _colliderControllers = new List<ColliderController>();
    
    public enum LayerIndex
    {
        PhysicsA = 6,
        PhysicsB = 7,
        PhysicsC = 8
    }

    public LayerIndex layerIndex;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerCollider = _player.GetComponent<CapsuleCollider>();
        
        BoxCollider[] _colliders = FindObjectsByType<BoxCollider>(FindObjectsSortMode.None).Where(x => x.gameObject.layer == (int)layerIndex).ToArray();

        foreach (var c in _colliders)
        {
            ColliderController cc = c.gameObject.AddComponent<ColliderController>();
            _colliderControllers.Add(cc);
        }
    }

    void Update()
    {
        if (boxCollider.bounds.Contains(_player.transform.position))
        {
            playerCollider.excludeLayers = ~((1 << 0) | (1 << (int)layerIndex));
        }

        for (int i = 0; i < _colliderControllers.Count; i++)
        {
            bool intersects = boxCollider.bounds.Intersects(_colliderControllers[i].visualBounds);
            _colliderControllers[i].boxCollider.excludeLayers = intersects ? 0 : ~0;

            if (!intersects)
            {
                continue;
            }

            int containState = EntirelyContains(_colliderControllers[i].visualBounds);
            
            // Collider is not fully inside region, resize the box collider it
            if (containState != 0)
            {
                float scaleMult = 1f / _colliderControllers[i].transform.localScale.x;
                
                // Collider cut off on right
                if (containState == 1)
                {
                    float leftEdge = _colliderControllers[i].visualBounds.min.x;
                    float rightEdge = boxCollider.bounds.max.x;
                    float differenceSize = rightEdge - leftEdge;
                    float differenceCenter = boxCollider.bounds.max.x - _colliderControllers[i].visualBounds.center.x;

                    _colliderControllers[i].boxCollider.size = new Vector3(differenceSize * scaleMult, 1f, 1f);
                    _colliderControllers[i].boxCollider.center = new Vector3((differenceCenter - differenceSize * .5f) * scaleMult, 0f, 0f);
                }
                else // Collider cut off on left
                {
                    float leftEdge = boxCollider.bounds.min.x;
                    float rightEdge = _colliderControllers[i].visualBounds.max.x;
                    float differenceSize = rightEdge - leftEdge;
                    
                    
                    float differenceCenter = _colliderControllers[i].visualBounds.center.x - boxCollider.bounds.min.x;

                    if (differenceSize < 0)
                    {
                        Debug.Log(_colliderControllers[i].gameObject.name +" ::: "+_colliderControllers[i].gameObject.transform.parent.name);
                    }
                    
                    _colliderControllers[i].boxCollider.size = new Vector3(differenceSize * scaleMult, 1f, 1f);
                    _colliderControllers[i].boxCollider.center = new Vector3((differenceSize * .5f - differenceCenter) * scaleMult, 0f, 0f);
                }
            }
            else
            {
                _colliderControllers[i].boxCollider.center = Vector3.zero;
                _colliderControllers[i].boxCollider.size = Vector3.one;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = regionColor;
        Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        
    }

    private int EntirelyContains(Bounds b)
    {
        if (!boxCollider.bounds.Contains(b.min))
        {
            return -1;
        }

        if (!boxCollider.bounds.Contains(b.max))
        {
            return 1;
        }


        return 0;
    }
}
