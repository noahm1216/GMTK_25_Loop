using UnityEngine;
using TMPro;

public class CollisionDebugUIManager : MonoBehaviour
{
    public static CollisionDebugUIManager Instance;

    public TextMeshProUGUI debugText;
    
    private int collisionCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void UpdateDebugText(string message)
    {
        collisionCount++;
        if (debugText != null)
        {
            //debugText.text = $"[{collisionCount}] {message}";
            debugText.text = message;
        }
    }
}