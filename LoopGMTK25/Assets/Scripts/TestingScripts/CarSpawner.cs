using TMPro;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public static CarSpawner Instance;
    
    public GameObject prefabToSpawn;         // The prefab to instantiate
    public Transform spawnLocation;          // Optional: location to spawn at (defaults to spawner's position)
    public PathFollower pathFollowerScript;
    public Transform carContainer;
    [HideInInspector]public int followerCount = 0;
    //public int FollowerCount => followerCount;
    public TMP_Text carCounterText;

    private GameObject lastSpawnedCar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // This function can be hooked up to a UI Button
    public void SpawnPrefab()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("No prefab assigned to spawn.");
            return;
        }

        Vector3 position = spawnLocation != null ? spawnLocation.position : transform.position;
        Quaternion rotation = spawnLocation != null ? spawnLocation.rotation : Quaternion.identity;

        lastSpawnedCar = Instantiate(prefabToSpawn, position, rotation, carContainer);
        
        followerCount++;
        carCounterText.text = $"Car Counter: {followerCount}";
        lastSpawnedCar.name = $"Car_{followerCount}";
        
        pathFollowerScript.AddFollower(lastSpawnedCar.transform);
    }
}