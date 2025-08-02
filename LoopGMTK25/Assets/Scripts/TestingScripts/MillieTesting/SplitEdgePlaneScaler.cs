using UnityEngine;

public class StretchTwoPlanesFromControl : MonoBehaviour
{
    public Transform leftPlane;
    public Transform rightPlane;
    public Transform control;

    private const float meshWidth = 10f; // Unity's built-in plane is 10 units wide

    private Vector3 leftInitialPos, rightInitialPos;
    private Vector3 leftInitialScale, rightInitialScale;

    private float fixedLeftEdgeX;
    private float fixedRightEdgeX;

    void Start()
    {
        // Cache initial positions and scales
        leftInitialPos = leftPlane.position;
        rightInitialPos = rightPlane.position;

        leftInitialScale = leftPlane.localScale;
        rightInitialScale = rightPlane.localScale;

        // Compute initial world-space edge positions
        float leftWorldWidth = leftInitialScale.x * meshWidth;
        float rightWorldWidth = rightInitialScale.x * meshWidth;

        fixedLeftEdgeX = leftInitialPos.x - leftWorldWidth / 2f;
        fixedRightEdgeX = rightInitialPos.x + rightWorldWidth / 2f;
    }

    void Update()
    {
        float controlX = Mathf.Clamp(control.position.x, fixedLeftEdgeX, fixedRightEdgeX);

        // LEFT PLANE
        float leftWidth = controlX - fixedLeftEdgeX;
        float leftScaleX = leftWidth / meshWidth;
        float leftCenterX = fixedLeftEdgeX + leftWidth / 2f;

        leftPlane.localScale = new Vector3(leftScaleX, leftInitialScale.y, leftInitialScale.z);
        leftPlane.position = new Vector3(leftCenterX, leftInitialPos.y, leftInitialPos.z);

        // RIGHT PLANE
        float rightWidth = fixedRightEdgeX - controlX;
        float rightScaleX = rightWidth / meshWidth;
        float rightCenterX = controlX + rightWidth / 2f;

        rightPlane.localScale = new Vector3(rightScaleX, rightInitialScale.y, rightInitialScale.z);
        rightPlane.position = new Vector3(rightCenterX, rightInitialPos.y, rightInitialPos.z);
    }
}