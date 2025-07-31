using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private int levelGoal = 5;
    private float goalSuccessTimer = 3.0f;

    public GameObject victoryUI;
    public TMP_Text victoryTimer;
    public GameObject carSpawner;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CollisionDebugUIManager.Instance.restartUI.activeSelf)
        {
            if (CarSpawner.Instance.followerCount >= levelGoal)
            {
                goalSuccessTimer -= Time.deltaTime;
                if (!victoryTimer.gameObject.activeSelf)
                {
                    victoryTimer.gameObject.SetActive(true);
                }

                if (goalSuccessTimer > 0.0f)
                {
                    victoryTimer.text = $"Time to Victory: {goalSuccessTimer}";
                }

                if (goalSuccessTimer <= 0.0f)
                {
                    victoryTimer.text = $"Time to Victory: 0";
                    Victory();
                }
            }
        }
    }

    void Victory()
    {
        carSpawner.SetActive(false);
        victoryUI.SetActive(true);
    }
}
