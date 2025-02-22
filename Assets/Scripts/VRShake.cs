using UnityEngine;
using System.Collections;

public class VRShake : MonoBehaviour
{
    public float shakeMagnitude = 0.2f;
    public float shakeDuration = 30f; // Set to 30 seconds
    public float shakeFrequency = 20f;

    private Transform _cameraTransform;
    private Vector3 _originalCameraPosition;
    private bool _isShaking = false; // Flag to track if shaking is active

    private void Start()
    {
        _cameraTransform = transform;

        if (_cameraTransform == null)
        {
            Debug.LogError("Camera Transform not found. Make sure this script is attached to the Camera Offset object.");
            enabled = false;
            return;
        }

        _originalCameraPosition = _cameraTransform.localPosition;
        StartShake();
    }

    public void StartShake()  // Renamed to StartShake
    {
        if (!_isShaking) // Prevent restarting if already shaking
        {
            _isShaking = true;
            StartCoroutine(ShakeCoroutine());
        }
    }

    public void StopShake() // Function to stop the shake
    {
        _isShaking = false;
        _cameraTransform.localPosition = _originalCameraPosition; // Restore position immediately
        StopCoroutine(ShakeCoroutine()); // Important: Stop the coroutine
    }


    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = _originalCameraPosition; // Initialize target position

        while (elapsedTime < shakeDuration && _isShaking)
        {
            // Smoothly interpolate to the last random shake position
            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, targetPosition, Time.deltaTime * shakeFrequency);

            // Calculate the next target shake position. This is done here rather than every frame to make the shake more predictable.
            if (Vector3.Distance(_cameraTransform.localPosition, targetPosition) < 0.01f)
            {
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;
                targetPosition = _originalCameraPosition + new Vector3(x, y, 0);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(SmoothReturn()); // Smoothly return after shake ends
        _isShaking = false;
    }

    private IEnumerator SmoothReturn()
    {
        while (Vector3.Distance(_cameraTransform.localPosition, _originalCameraPosition) > 0.01f) // Return until close enough
        {
            _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _originalCameraPosition, Time.deltaTime * shakeFrequency);
            yield return null;
        }
        _cameraTransform.localPosition = _originalCameraPosition; // Ensure exact original position
    }
}