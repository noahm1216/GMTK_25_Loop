using UnityEngine;
using UnityEngine.UI;

public class CameraEffectTester : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SimpleCameraEffects _cameraEffects;
    [SerializeField] private Transform _testTarget;
    [SerializeField] private Text _statusText;
    [SerializeField] private float _zoomDuration;
    [SerializeField] private float _zoomOutDuration;
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakeIntensity;

    [Header("Key Bindings")]
    [SerializeField] private KeyCode _zoomKey = KeyCode.Z;
    [SerializeField] private KeyCode _zoomOutKey = KeyCode.O;
    [SerializeField] private KeyCode _shakeKey = KeyCode.S;
    [SerializeField] private KeyCode _resetKey = KeyCode.R;
    [SerializeField] private KeyCode _nextPresetKey = KeyCode.T;

    private void Update()
    {
        HandleInput();
        UpdateUI();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(_zoomKey))
        {
            _cameraEffects.PlayZoomEffect(_zoomDuration);
        }
        if (Input.GetKeyDown(_zoomOutKey))
        {
            _cameraEffects.PlayZoomOutEffect(_zoomOutDuration);
        }

        if (Input.GetKeyDown(_shakeKey))
        {
            _cameraEffects.PlayShakeEffect(_shakeDuration, _shakeIntensity);
        }

        if (Input.GetKeyDown(_resetKey))
        {
            _cameraEffects.ResetAllCameras();
        }

        if (Input.GetKeyDown(_nextPresetKey))
        {
            int nextPreset = (_cameraEffects.GetCurrentPresetIndex() + 1) % 3;
            _cameraEffects.ApplyPreset(nextPreset);
        }
    }

    private void UpdateUI()
    {
        if (_statusText == null) return;

        _statusText.text = $"CAMERA EFFECT TESTER\n" +
                          $"Current Transition: {_cameraEffects.GetCurrentPresetName()}\n\n" +
                          $"CONTROLS:\n" +
                          $"{_zoomKey} - Zoom Effect\n" +
                          $"{_zoomOutKey} - Zoom Out Effect\n" +
                          $"{_shakeKey} - Shake Effect\n" +
                          $"{_resetKey} - Reset Camera\n" +
                          $"{_nextPresetKey} - Next Transition Style";
    }
}