using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class PlayerHeightAdjuster : MonoBehaviour
{


    [Header("Collider Tag")]
    [Tooltip("Tag assigned to the sitting zone collider.")]
    public string sittingZoneTag = "SittingZone";





    void Start()
    {


    }

    /// <summary>
    /// Adjusts the player's height by modifying the XR Origin's camera offset.
    /// </summary>
    /// <param name="height">Desired height in meters.</param>


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