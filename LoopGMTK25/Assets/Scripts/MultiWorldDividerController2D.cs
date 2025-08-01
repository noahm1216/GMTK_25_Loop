using UnityEngine;
using System.Collections.Generic;

public class MultiWorldDividerController2D : MonoBehaviour
{
    [Header("Divider Settings")]
    public List<Transform> dividers; // Must be sorted left to right in Inspector
    public float minX = 0f;
    public float maxX = 10f;
    public float minDistanceBetweenDividers = 1f;

    [Header("World Parents")]
    public List<Transform> worldParents; // Must be ordered to match zones

    private Camera cam;
    private Transform draggingDivider = null;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleDrag();
        UpdateWorldVisibility();
    }

    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            foreach (Transform divider in dividers)
            {
                if (Mathf.Abs(mousePos.x - divider.position.x) < 0.3f)
                {
                    draggingDivider = divider;
                    break;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            draggingDivider = null;
        }

        if (draggingDivider != null)
        {
            int index = dividers.IndexOf(draggingDivider);

            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            float rawX = mouseWorldPos.x;

            // Define exclusive valid movement bounds for this divider
            float leftLimit = (index == 0) ? minX : dividers[index - 1].position.x + minDistanceBetweenDividers;
            float rightLimit = (index == dividers.Count - 1) ? maxX : dividers[index + 1].position.x - minDistanceBetweenDividers;

            // Clamp and snap
            rawX = Mathf.Clamp(rawX, leftLimit, rightLimit);
            float snappedX = Mathf.Round(rawX) + 0.5f;

            draggingDivider.position = new Vector3(snappedX, draggingDivider.position.y, -1);
        }
    }


    void UpdateWorldVisibility()
    {
        List<float> borders = new List<float>();
        borders.Add(minX); // Start of world 0
        foreach (Transform divider in dividers)
            borders.Add(divider.position.x);
        borders.Add(maxX); // End of last world

        for (int i = 0; i < worldParents.Count; i++)
        {
            float left = borders[i];
            float right = borders[i + 1];

            foreach (Transform child in worldParents[i])
            {
                float x = child.position.x;
                bool visible = (x >= left && x < right);
                child.gameObject.SetActive(visible);
            }
        }
    }
}
