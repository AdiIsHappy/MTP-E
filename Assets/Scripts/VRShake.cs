using Unity.XR.CoreUtils;
using UnityEngine;

public class VRShake : MonoBehaviour
{
    public float shakeMagnitude = 0.2f;
    public float shakeDuration = 30f;
    public float shakeFrequency = 20f;

    private Transform _cameraTransform;
    private Vector3 _originalCameraPosition;
    private float _shakeTimer = 0f;
    private Vector3 _targetShakePosition;
    private XROrigin _xrOrigin;

    // Instead of a boolean flag, use a counter.  0 means shaking is enabled.
    private int _shakeDisableRequests = 0;

    private void Start()
    {
        _cameraTransform = transform;

        if (_cameraTransform == null)
        {
            Debug.LogError("Camera Transform not found. Make sure this script is attached to the Camera Offset object.");
            enabled = false;
            return;
        }

        _xrOrigin = GameObject.FindFirstObjectByType<XROrigin>();
        if (_xrOrigin == null)
        {
            Debug.LogError("XROrigin not found. Make sure this script is attached to the Origin Object.");
            enabled = false;
            return;
        }

        _originalCameraPosition = _cameraTransform.localPosition;
        StartShake();
    }

    public void StartShake()
    {
        // Only start shaking if not currently disabled.
        if (_shakeDisableRequests == 0)
        {
            _shakeTimer = 0f;
            SetNewTargetShakePosition();
        }
    }

    public void StopShake()
    {
        _shakeTimer = 0f;
        _cameraTransform.localPosition = _originalCameraPosition;
    }

    // Call this to temporarily disable shaking.
    public void DisableShake()
    {
        _shakeDisableRequests++;
    }

    // Call this to re-enable shaking.
    public void EnableShake()
    {
        _shakeDisableRequests--;
        if (_shakeDisableRequests < 0)
        {
            _shakeDisableRequests = 0; // Ensure it doesn't go negative.
        }

        // If shaking was previously disabled and now re-enabled, start it.
        if (_shakeDisableRequests == 0)
        {
            StartShake();
        }
    }


    private void Update()
    {
        // Only shake if not disabled.
        if (_shakeDisableRequests == 0)
        {
            _shakeTimer += Time.deltaTime;

            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _targetShakePosition, Time.deltaTime * shakeFrequency);

            if (Vector3.Distance(_cameraTransform.localPosition, _targetShakePosition) < 0.01f)
            {
                SetNewTargetShakePosition();
            }

            if (_shakeTimer >= shakeDuration)
            {
                StopShake(); // No need for coroutine, just stop immediately.
            }
        }
    }

    private void SetNewTargetShakePosition()
    {
        var x = Random.Range(-1f, 1f) * shakeMagnitude;
        var y = Random.Range(-1f, 1f) * shakeMagnitude;
        _targetShakePosition = new Vector3(x, y + _xrOrigin.CameraYOffset, 0);
    }
}