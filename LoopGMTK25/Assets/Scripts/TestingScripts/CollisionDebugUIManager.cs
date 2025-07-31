using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CollisionDebugUIManager : MonoBehaviour
{
    public static CollisionDebugUIManager Instance;

    public TextMeshProUGUI debugText;

    public GameObject restartUI;

    public TMP_Text score;
    
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

    public void CollidedUpdateGameState(string message)
    {
        collisionCount++;
        if (debugText != null)
        {
            //debugText.text = $"[{collisionCount}] {message}";
            debugText.text = message;
        }

        score.text = $"Score: {CarSpawner.Instance.followerCount - 1} car(s)";

        PathFollower.Instance.enabled = false;
        restartUI.SetActive(true);
        GameManager.Instance.carSpawner.SetActive(false);
    }

    public void RestartLevel()
    {
        CarSpawner.Instance.followerCount = 0;
        SceneManager.LoadScene("MillieTestScene");
    }
}