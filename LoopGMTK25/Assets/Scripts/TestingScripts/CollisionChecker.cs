using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    // Optional: Tag to identify similar prefabs
    public string prefabTag = "Follower";

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the other object has the same tag
        if (collision.gameObject.CompareTag(prefabTag))
        {
            // Prevent duplicate logs by only logging from the object with the lower ID
            if (GetInstanceID() < collision.gameObject.GetInstanceID())
            {
                string message = $"{name} collided with {collision.gameObject.name}";
                Debug.Log(message);
                CollisionDebugUIManager.Instance?.CollidedUpdateGameState(message);
            }
        }
    }
}