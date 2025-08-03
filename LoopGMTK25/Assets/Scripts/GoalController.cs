using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GoalController : MonoBehaviour
{
    [Header("Win Time Settings")]
    public float maxTime = 3f;
    private float counter = 0f;
    private bool isInside = false;
    private bool goalReached = false;
    private AudioClipHolder audioClips;

    public UnityEvent onStart, onHovering, onExit, onWin;

    private void Start()
    {
        audioClips = GetComponent<AudioClipHolder>();
        onStart?.Invoke();
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
                SimpleCameraEffects.Instance.ActivateFinishCam();
                onWin?.Invoke();
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
            onExit?.Invoke();
            isInside = false;
            ResetCounter();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onHovering?.Invoke();
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
