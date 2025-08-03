using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private Transform parent;
    private Vector2 originalPosition;

    private List<RectTransform> siblings = new List<RectTransform>();
    private List<Vector2> slotPositions = new List<Vector2>();

    public Region region;
    
    private int _dragStartIndex;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        parent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        // Record all siblings and their positions
        siblings.Clear();
        slotPositions.Clear();


        foreach (Transform child in parent)
        {
            RectTransform rt = child as RectTransform;
            siblings.Add(rt);
            slotPositions.Add(rt.anchoredPosition);
        }

        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move X only
        Vector2 delta = eventData.delta / canvas.scaleFactor;
        rectTransform.anchoredPosition += new Vector2(delta.x, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Snap to nearest slot
        int closestSlot = GetClosestSlotIndex(rectTransform.anchoredPosition);

        // Move this to the slot
        rectTransform.anchoredPosition = slotPositions[closestSlot];

        UIDraggable replaced = siblings[closestSlot].gameObject.GetComponent<UIDraggable>();
        
        // Reorder siblings to match new order
        ReorderSiblings(closestSlot);
        
        WorldChangeController.RaiseWorldSwitchEvent();

        
    }

    private int GetClosestSlotIndex(Vector2 position)
    {
        int index = 0;
        float minDist = float.MaxValue;

        for (int i = 0; i < slotPositions.Count; i++)
        {
            float dist = Mathf.Abs(position.x - slotPositions[i].x);
            if (dist < minDist)
            {
                minDist = dist;
                index = i;
            }
        }
        return index;
    }

    private void ReorderSiblings(int newIndex)
    {
        siblings.Remove(rectTransform);
        siblings.Insert(newIndex, rectTransform);

        // Snap all to their assigned slot
        for (int i = 0; i < siblings.Count; i++)
        {
            siblings[i].anchoredPosition = slotPositions[i];
        }
    }
}