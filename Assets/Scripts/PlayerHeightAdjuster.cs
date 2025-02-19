using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class PlayerHeightAdjuster : MonoBehaviour
{
    [Header("Height Settings")]
    [Tooltip("Default standing height of the player.")]
    public float defaultHeight = 1.8f; // Adjust based on your default setup

    [Tooltip("Sitting height to set when in the trigger.")]
    public float sittingHeight = 1.0f;

    [Header("Collider Tag")]
    [Tooltip("Tag assigned to the sitting zone collider.")]
    public string sittingZoneTag = "SittingZone";

    private XROrigin xrOrigin;
    private bool isSitting = false;



    void Start()
    {
        // Get the XR Origin component
        xrOrigin = GetComponent<XROrigin>();
        if (xrOrigin == null)
        {
            Debug.LogError("PlayerHeightAdjuster: No XROrigin component found on the GameObject.");
        }

        // Optionally set default height at start
        SetPlayerHeight(defaultHeight);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(sittingZoneTag))
        {
            SetPlayerHeight(sittingHeight);
            isSitting = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(sittingZoneTag))
        {
            SetPlayerHeight(defaultHeight);
            isSitting = false;
        }
    }

    /// <summary>
    /// Adjusts the player's height by modifying the XR Origin's camera offset.
    /// </summary>
    /// <param name="height">Desired height in meters.</param>
    void SetPlayerHeight(float height)
    {
        if (xrOrigin != null)
        {
            // Update the XR Origin's camera height
            xrOrigin.CameraYOffset = height;

            // Optionally, reset the tracking space
            //xrOrigin.MakeOriginAtCameraPos();
        }
    }

    // Optional: Visual feedback or additional logic can be added here
    void Update()
    {
        // Example: Smoothly interpolate height changes
        /*
        float targetHeight = isSitting ? sittingHeight : defaultHeight;
        xrOrigin.CameraInOriginSpaceHeight = Mathf.Lerp(xrOrigin.CameraInOriginSpaceHeight, targetHeight, Time.deltaTime * 5f);
        */
    }


}