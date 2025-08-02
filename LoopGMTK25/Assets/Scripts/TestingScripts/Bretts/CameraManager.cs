using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }
    private CinemachineVirtualCamera virtualCam;
    //private CinemachineBasicMultiChannelPerlin noise;
    private float originalOrthoSize;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        virtualCam = GetComponent<CinemachineVirtualCamera>();
        //noise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //originalOrthoSize = virtualCam.m_Lens.OrthographicSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Shake test
        {
            //ShakeCamera(.5f, .1f, 0.3f);  
        }

        if (Input.GetKeyDown(KeyCode.Z)) // Zoom in test
        {
            ZoomCamera(7f, .5f, .8f);
        }

        if (Input.GetKeyDown(KeyCode.X)) // Zoom out test
        {
            ZoomCamera(8f, .5f, .8f);
        }
    }

    /*public void ShakeCamera(float amplitude, float frequency, float time)
    {
        StartCoroutine(ShakeRoutine(amplitude, frequency, time));
    }

    private IEnumerator ShakeRoutine(float amplitude, float frequency, float time)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;
        yield return new WaitForSeconds(time);
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }*/

    public void ZoomCamera(float targetOrthoSize, float zoomDuration, float returnDuration)
    {
        StartCoroutine(ZoomRoutine(targetOrthoSize, zoomDuration, returnDuration));
    }


    private IEnumerator ZoomRoutine(float targetOrthoSize, float zoomDuration, float returnDuration)
    {
        float startSize = virtualCam.m_Lens.OrthographicSize;
        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetOrthoSize, elapsed / zoomDuration);
            yield return null;
        }

        //yield return new WaitForSeconds(0.5f); // Small delay before returning to original size

        elapsed = 0f;
        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(targetOrthoSize, originalOrthoSize, elapsed / returnDuration);
            yield return null;
        }
    }
}

