using Cinemachine;
using UnityEngine;
using System.Collections;

[System.Serializable]
public struct TransitionPreset
{
    public string name;
    public CinemachineBlendDefinition.Style style;
    public float duration;
}

public class SimpleCameraEffects : MonoBehaviour
{
    public static SimpleCameraEffects Instance { get; private set; }
   
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera _baseCamera;
    [SerializeField] private CinemachineVirtualCamera _zoomCamera;
    [SerializeField] private CinemachineVirtualCamera _zoomOutCamera;
    [SerializeField] private CinemachineVirtualCamera _finishCamera;
    [SerializeField] private CinemachineVirtualCamera _shakeCamera;

    [Header("Transition Presets")]
    [SerializeField]
    private TransitionPreset[] _transitionPresets = {
        new TransitionPreset { name = "Instant", style = CinemachineBlendDefinition.Style.Cut, duration = 0f },
        new TransitionPreset { name = "Quick", style = CinemachineBlendDefinition.Style.EaseInOut, duration = 0.3f },
        new TransitionPreset { name = "Smooth", style = CinemachineBlendDefinition.Style.EaseInOut, duration = 1f }
    };

    private CinemachineBrain _brain;
    private int _currentPresetIndex = 1;
    private float _originalOrthoSize;

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
        _brain = Camera.main.GetComponent<CinemachineBrain>();
        _originalOrthoSize = _baseCamera.m_Lens.OrthographicSize;
        ResetAllCameras();
        ApplyPreset(1);
    }

    // ===== CORE FUNCTIONS =====

    public void PlayZoomEffect(float duration)
    {
        StartCoroutine(ZoomRoutine(duration));
    }

    public void ActivateZoomCam()
    {
        ApplyPreset(1);
        _zoomCamera.Priority = 20; 
    }

    public void ActivateZoomOutCam()
    {
        ApplyPreset(2);
        _zoomOutCamera.Priority = 20;
        
    }

    public void ActivateFinishCam()
    {
        ApplyPreset(4);
        _finishCamera.Priority = 20;
    }

    public void PlayZoomOutEffect(float duration)
    {
        StartCoroutine(ZoomOutRoutine(duration));
    }


    public void PlayShakeEffect(float duration, float intensity)
    {
        var noise = _shakeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null) return;

        StartCoroutine(ShakeRoutine(noise, intensity, duration));
    }

    private IEnumerator ShakeRoutine(CinemachineBasicMultiChannelPerlin noise, float intensity, float duration)
    {
        noise.m_AmplitudeGain = intensity;
        _shakeCamera.Priority = 20;

        yield return new WaitForSeconds(duration);

        noise.m_AmplitudeGain = 0;
        _shakeCamera.Priority = 0;
    }

    private IEnumerator ZoomRoutine(float duration)
    {
        
        _zoomCamera.Priority = 20;

        yield return new WaitForSeconds(duration);

        _zoomCamera.Priority = 0;
    }

    private IEnumerator ZoomOutRoutine(float duration)
    {
        _zoomOutCamera.Priority = 20;

        yield return new WaitForSeconds(duration);

        _zoomOutCamera.Priority = 0;

    }

    // ===== UTILITIES =====
    public void SetCustomTransition(CinemachineBlendDefinition.Style style, float duration)
    {
        _brain.m_DefaultBlend = new CinemachineBlendDefinition(style, duration);
    }

    public void ResetAllCameras()
    {
        ApplyPreset(3);
        _baseCamera.Priority = 10;
        _zoomCamera.Priority = 0;
        _zoomOutCamera.Priority = 0;
        _shakeCamera.Priority = 0;
        _finishCamera.Priority = 0;
    }

    public void ForceReset()
    {
        StopAllCoroutines();
        ResetAllCameras(); // Sets priorities: Base=10, Others=0
    }

    // ===== PRESET CONTROL =====
    public int GetCurrentPresetIndex() => _currentPresetIndex;
    public string GetCurrentPresetName() => _transitionPresets[_currentPresetIndex].name;
    public void ApplyPreset(int index)
    {
        if (index < 0 || index >= _transitionPresets.Length) return;
        _currentPresetIndex = index;
        SetCustomTransition(_transitionPresets[index].style, _transitionPresets[index].duration);
    }
}