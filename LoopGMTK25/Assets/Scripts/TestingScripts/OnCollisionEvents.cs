using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionEvents : MonoBehaviour
{
    public LayerMask allowedKeyLayers;

    public UnityEvent onAnyCollisionEvent, onKeyCollisionEvent;


    private void OnCollisionEnter(Collision col)
    {
        onAnyCollisionEvent?.Invoke();

        if (IsLayerInMask(allowedKeyLayers, col.gameObject.layer)) onKeyCollisionEvent?.Invoke();
    }

    public bool IsLayerInMask(LayerMask mask, int layer)
    {
        return ((mask.value & (1 << layer)) != 0);
    }
}
