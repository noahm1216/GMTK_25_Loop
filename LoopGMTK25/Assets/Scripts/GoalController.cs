using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GoalController : MonoBehaviour
{
    [Header("Win Time Settings")]
    public float maxTime = 3f;
    private float counter = 0f;
    private bool isInside = false;
    private bool goalReached = false;
    private AudioClipHolder audioClips;

    private void Start()
    {
        audioClips = GetComponent<AudioClipHolder>();   
    }
    private void Update()
    {
        if (isInside && !goalReached)
        {
            IncrementCounter();
            //counterText.text = counter.ToString("F2");

            if (counter >= maxTime)
            {
                // Optional: Trigger goal reached logic here
                
                counter = maxTime;
                Debug.Log("Goal reached!");
                goalReached = true;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
            ResetCounter();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
        }
        //Play some Audio maybe...
    }

    private void IncrementCounter()
    {
        counter += Time.deltaTime;
    }

    private void ResetCounter()
    {
        if(goalReached) return;
        counter = 0f;
        //counterText.text = counter.ToString("F2");
    }

   }
