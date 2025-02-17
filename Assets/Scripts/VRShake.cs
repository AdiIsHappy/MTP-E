using UnityEngine;
using System.Collections;

public class VRShake : MonoBehaviour
{
    public float shakeMagnitude = 0.2f;
    public float shakeDuration = 30f; // Set to 30 seconds
    public float shakeFrequency = 20f;

    private Transform cameraTransform;
    private Vector3 originalCameraPosition;
    private bool isShaking = false; // Flag to track if shaking is active

    private void Start()
    {
        cameraTransform = transform;

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not found. Make sure this script is attached to the Camera Offset object.");
            enabled = false;
            return;
        }

        originalCameraPosition = cameraTransform.localPosition;
        StartShake();
    }

    public void StartShake()  // Renamed to StartShake
    {
        if (!isShaking) // Prevent restarting if already shaking
        {
            isShaking = true;
            StartCoroutine(ShakeCoroutine());
        }
    }

    public void StopShake() // Function to stop the shake
    {
        isShaking = false;
        cameraTransform.localPosition = originalCameraPosition; // Restore position immediately
        StopCoroutine(ShakeCoroutine()); // Important: Stop the coroutine
    }


    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 targetPosition = originalCameraPosition; // Initialize target position

        while (elapsedTime < shakeDuration && isShaking)
        {
            // Smoothly interpolate to the last random shake position
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetPosition, Time.deltaTime * shakeFrequency);

            // Calculate the next target shake position. This is done here rather than every frame to make the shake more predictable.
            if (Vector3.Distance(cameraTransform.localPosition, targetPosition) < 0.01f)
            {
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;
                targetPosition = originalCameraPosition + new Vector3(x, y, 0);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(SmoothReturn()); // Smoothly return after shake ends
        isShaking = false;
    }

    private IEnumerator SmoothReturn()
    {
        while (Vector3.Distance(cameraTransform.localPosition, originalCameraPosition) > 0.01f) // Return until close enough
        {
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, originalCameraPosition, Time.deltaTime * shakeFrequency);
            yield return null;
        }
        cameraTransform.localPosition = originalCameraPosition; // Ensure exact original position
    }
}