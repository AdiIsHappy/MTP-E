using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference rightTriggerAction;
    [SerializeField] private InputActionReference leftTriggerAction;
    [Header("Height Settings")]
    [Tooltip("Default standing height of the player.")]
    public float defaultHeight = 1.8f;
    [Tooltip("Sitting height to set when in the trigger.")]
    public float sittingHeight = 1.0f;
    [Tooltip("Time required to perform sitting in seconds.")]
    public float sittingDuration = 0.1f;

    private XROrigin _xrOrigin;
    private bool _isSitting = false;
    private Coroutine _sittingCoroutine;
    private VRShake _vrShake;

    private void Start()
    {
        if (rightTriggerAction == null || leftTriggerAction == null)
        {
            Debug.LogError("Trigger Action for sitting are not set");
        }
        _xrOrigin = GetComponent<XROrigin>();
        if (_xrOrigin == null)
        {
            Debug.LogError("No XROrigin component found on the GameObject.");
        }
        _vrShake = GetComponentInChildren<VRShake>(); // Get the VRShake component.  Adjust if needed.
        if (_vrShake == null)
        {
            Debug.LogError("VRShake component not found!");
        }

        SetPlayerHeight(defaultHeight);
    }

    private void SetPlayerHeight(float height)
    {
        _xrOrigin.CameraYOffset = height;
    }

    private void Update()
    {
        if (rightTriggerAction.action.triggered || leftTriggerAction.action.triggered)
        {
            if (_sittingCoroutine != null)
            {
                StopCoroutine(_sittingCoroutine);
            }
            
            if (_vrShake != null)
            {
                _vrShake.DisableShake();
            }

            _sittingCoroutine = StartCoroutine(LerpHeight(_isSitting ? defaultHeight : sittingHeight));
            _isSitting = !_isSitting;
        }
    }

    private IEnumerator LerpHeight(float targetHeight)
    {
        float startHeight = _xrOrigin.CameraYOffset;
        float timer = 0f;

        while (timer < sittingDuration)
        {
            timer += Time.deltaTime;
            float newHeight = Mathf.Lerp(startHeight, targetHeight, timer / sittingDuration);
            SetPlayerHeight(newHeight);
            yield return null;
        }

        SetPlayerHeight(targetHeight);
        _sittingCoroutine = null;
        if (_vrShake != null)
        {
            _vrShake.EnableShake();
        }
    }
}