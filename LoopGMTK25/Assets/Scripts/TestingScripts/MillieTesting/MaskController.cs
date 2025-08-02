using UnityEngine;

public class MaskController : MonoBehaviour
{
    public Transform leftPlane;
    public Transform centerPlane;
    public Transform rightPlane;

    [Header("Controls")]
    public float innerEdgeL = -3f; // X position of Left↔Center edge
    public float innerEdgeR = 3f;  // X position of Center↔Right edge

    private float meshWidth = 10f; // Unity's default plane mesh width
    private Vector3 leftInitialPos, centerInitialPos, rightInitialPos;
    private Vector3 leftInitialScale, centerInitialScale, rightInitialScale;
    private float fixedLeftEdgeX, fixedRightEdgeX;

    void Start()
    {
        // Cache initial transforms
        leftInitialPos = leftPlane.position;
        centerInitialPos = centerPlane.position;
        rightInitialPos = rightPlane.position;

        leftInitialScale = leftPlane.localScale;
        centerInitialScale = centerPlane.localScale;
        rightInitialScale = rightPlane.localScale;

        // Calculate world-space fixed outer edges
        fixedLeftEdgeX = leftInitialPos.x - (leftInitialScale.x * meshWidth * 0.5f);
        fixedRightEdgeX = rightInitialPos.x + (rightInitialScale.x * meshWidth * 0.5f);
    }

    void Update()
    {
        ApplyEdgeControls();
    }

    void ApplyEdgeControls()
    {
        // Clamp inner edges between outer edges to avoid inversion
        innerEdgeL = Mathf.Clamp(innerEdgeL, fixedLeftEdgeX, fixedRightEdgeX);
        innerEdgeR = Mathf.Clamp(innerEdgeR, innerEdgeL, fixedRightEdgeX);

        // Compute widths
        float leftWidth = innerEdgeL - fixedLeftEdgeX;
        float centerWidth = innerEdgeR - innerEdgeL;
        float rightWidth = fixedRightEdgeX - innerEdgeR;

        // Convert widths to local scale
        float leftScaleX = leftWidth / meshWidth;
        float centerScaleX = centerWidth / meshWidth;
        float rightScaleX = rightWidth / meshWidth;

        // Set positions (centered on each region)
        float leftCenterX = fixedLeftEdgeX + leftWidth * 0.5f;
        float centerCenterX = innerEdgeL + centerWidth * 0.5f;
        float rightCenterX = innerEdgeR + rightWidth * 0.5f;

        // Apply transforms
        leftPlane.position = new Vector3(leftCenterX, leftInitialPos.y, leftInitialPos.z);
        centerPlane.position = new Vector3(centerCenterX, centerInitialPos.y, centerInitialPos.z);
        rightPlane.position = new Vector3(rightCenterX, rightInitialPos.y, rightInitialPos.z);

        leftPlane.localScale = new Vector3(leftScaleX, leftInitialScale.y, leftInitialScale.z);
        centerPlane.localScale = new Vector3(centerScaleX, centerInitialScale.y, centerInitialScale.z);
        rightPlane.localScale = new Vector3(rightScaleX, rightInitialScale.y, rightInitialScale.z);
    }
}
