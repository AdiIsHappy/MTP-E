using Unity.XR.CoreUtils;
using UnityEngine;

public class VRShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeMagnitude = 0.2f;
    public float shakeDuration = 30f;
    public float shakeFrequency = 20f;

    private Transform _cameraTransform;
    private Vector3 _originalCameraPosition;
    private Vector3 _targetShakePosition;
    private XROrigin _xrOrigin;
    private EarthquakeManager _earthquakeManager;

    // Instead of a boolean flag, use a counter.  0 means shaking is enabled.
    private int _shakeDisableRequests = 0;

    private void Start()
    {
        _cameraTransform = transform;

        if (_cameraTransform == null)
        {
            Debug.LogError(
                "Camera Transform not found. Make sure this script is attached to the Camera Offset object."
            );
            enabled = false;
            return;
        }

        _xrOrigin = GameObject.FindFirstObjectByType<XROrigin>();
        if (_xrOrigin == null)
        {
            Debug.LogError(
                "XROrigin not found. Make sure this script is attached to the Origin Object."
            );
            enabled = false;
            return;
        }

        _earthquakeManager = GameObject.FindFirstObjectByType<EarthquakeManager>();
        if (_earthquakeManager == null)
        {
            Debug.LogError(
                "EarthquakeManager not found. Make sure this script is attached to the Origin Object."
            );
            enabled = false;
            return;
        }

        _originalCameraPosition = _cameraTransform.localPosition;
    }

    public void StopShake() { }

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
    }

    private void Update()
    {
        if (_earthquakeManager.IsEarthquakeHappening)
        {
            if (_shakeDisableRequests == 0)
            {
                _cameraTransform.localPosition = Vector3.Lerp(
                    _cameraTransform.localPosition,
                    _targetShakePosition,
                    Time.deltaTime * shakeFrequency
                );

                if (Vector3.Distance(_cameraTransform.localPosition, _targetShakePosition) < 0.01f)
                {
                    SetNewTargetShakePosition();
                }
            }
        }
        else
        {
            _cameraTransform.localPosition = _originalCameraPosition;
        }
    }

    private void SetNewTargetShakePosition()
    {
        var x = Random.Range(-1f, 1f) * shakeMagnitude * _earthquakeManager.EarthquakeMagnitude;
        var y = Random.Range(-1f, 1f) * shakeMagnitude * _earthquakeManager.EarthquakeMagnitude;
        _targetShakePosition = new Vector3(x, y + _xrOrigin.CameraYOffset, 0);
    }
}

public enum ObjectType
{
    Grounded,
    Hanging,
}
