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
    public List<Transform> worldParents; // Must be ordered to match zones[Header("World Sliding")]
    public int visibleWorldCount = 3;
    private int currentStartIndex = 0;  // Index of leftmost visible world
    
    [Header("Plane Zones")]
    public List<Transform> planes; // One more than dividers
    private List<Vector3> planeInitialScales = new List<Vector3>();
    private List<Vector3> planeInitialPositions = new List<Vector3>();

    private const float planeMeshWidth = 10f; // Unity default plane width


    private Camera cam;
    private Transform draggingDivider = null;

    void Start()
    {
        cam = Camera.main;
        
        planeInitialScales.Clear();
        planeInitialPositions.Clear();

        foreach (var plane in planes)
        {
            planeInitialScales.Add(plane.localScale);
            planeInitialPositions.Add(plane.position);
        }
    }

    void Update()
    {
        HandleDrag();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentStartIndex = (currentStartIndex - 1 + worldParents.Count) % worldParents.Count;
            UpdateWorldVisibility();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentStartIndex = (currentStartIndex + 1) % worldParents.Count;
            UpdateWorldVisibility();
        }

        //UpdateWorldVisibility();
        UpdatePlanesBetweenDividers();
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
            SimpleCameraEffects.Instance.ResetAllCameras();
            

        }

        if (draggingDivider != null)
        {

            SimpleCameraEffects.Instance.ActivateZoomCam();

            int index = dividers.IndexOf(draggingDivider);

            Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            float rawX = mouseWorldPos.x;

            // Calculate full clamped range
            float leftBound = (index == 0) ? minX + 1f : dividers[index - 1].position.x + minDistanceBetweenDividers;
            float rightBound = (index == dividers.Count - 1) ? maxX - 1f : dividers[index + 1].position.x - minDistanceBetweenDividers;

            rawX = Mathf.Clamp(rawX, leftBound, rightBound);

            // Snap to nearest unit
            float snappedX = Mathf.Round(rawX - 0.5f) + 0.5f;


            // Final clamp in case snapping pushes it out of bounds
            snappedX = Mathf.Clamp(rawX, leftBound, rightBound);

            draggingDivider.position = new Vector3(snappedX, draggingDivider.position.y, -1);
        }
    }

    void UpdateWorldVisibility()
    {
        int zones = visibleWorldCount;

        // Safety: make sure dividers.Count == visibleWorldCount - 1
        if (dividers.Count != zones - 1)
        {
            Debug.LogError("Divider count must be one less than visible world count.");
            return;
        }

        // Build bounds
        List<float> leftBounds = new List<float>();
        List<float> rightBounds = new List<float>();

        leftBounds.Add(minX); // first zone starts at minX

        for (int i = 0; i < dividers.Count; i++)
        {
            rightBounds.Add(dividers[i].position.x);           // end of zone i
            leftBounds.Add(dividers[i].position.x);            // start of zone i+1
        }

        rightBounds.Add(maxX); // last zone ends at maxX

        // Disable everything
        foreach (Transform world in worldParents)
        {
            foreach (Transform child in world)
            {
                child.gameObject.SetActive(false);
            }
        }

        // Show the N visible worlds starting from currentStartIndex
        for (int zone = 0; zone < zones; zone++)
        {
            int worldIndex = (currentStartIndex + zone) % worldParents.Count;
            Transform world = worldParents[worldIndex];

            float zoneLeft = leftBounds[zone];
            float zoneRight = rightBounds[zone];

            foreach (Transform child in world)
            {
                float x = child.position.x;
                bool visible = (x >= zoneLeft && x < zoneRight);
                child.gameObject.SetActive(visible);
            }
        }
    }
    
    void UpdatePlanesBetweenDividers()
    {
        if (planes.Count != dividers.Count + 1)
        {
            Debug.LogError("Plane count must be one more than divider count.");
            return;
        }

        // Build edge positions
        List<float> edgePositions = new List<float>();
        edgePositions.Add(minX);
        foreach (Transform divider in dividers)
        {
            edgePositions.Add(divider.position.x);
        }
        edgePositions.Add(maxX);

        for (int i = 0; i < planes.Count; i++)
        {
            float leftEdge = edgePositions[i];
            float rightEdge = edgePositions[i + 1];
            float segmentWidth = rightEdge - leftEdge;

            // Compute center of segment
            float segmentCenter = (leftEdge + rightEdge) * 0.5f;

            // Scale X to fill segment exactly
            float worldWidth = segmentWidth;
            float localScaleX = worldWidth / planeMeshWidth;

            // Apply
            Transform plane = planes[i];
            Vector3 originalScale = planeInitialScales[i];
            Vector3 originalPos = planeInitialPositions[i];

            plane.localScale = new Vector3(localScaleX, originalScale.y, originalScale.z);
            plane.position = new Vector3(segmentCenter, originalPos.y, originalPos.z);
        }
    }




}
