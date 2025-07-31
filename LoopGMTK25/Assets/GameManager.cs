using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int levelGoal = 5;
    private float goalSuccessTimer = 3.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CarSpawner.Instance.followerCount == levelGoal)
        {
            goalSuccessTimer -= Time.deltaTime;
            if (goalSuccessTimer <= 0.0f)
            {
                Victory();
            }
        }
    }

    void Victory()
    {
        
    }
}
