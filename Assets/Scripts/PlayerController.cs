using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionReference rightTriggerAction;

    [SerializeField]
    private InputActionReference leftTriggerAction;

    [Header("Height Settings")]
    [Tooltip("Default standing height of the player.")]
    public float defaultHeight = 1.8f;

    [Tooltip("Sitting height to set when in the trigger.")]
    public float sittingHeight = 1.0f;

    [Tooltip("Time required to perform sitting in seconds.")]
    public float sittingDuration = 0.1f;

    private XROrigin _xrOrigin;
    public bool isSitting = false;
    private Coroutine _sittingCoroutine;
    private VRShake _vrShake;
    private bool _isInSittingZone = false;

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

    private void OggerEnter(Collider other)
    {
        if (other.CompareTag("SittingZone"))
        {
            UserManager._instance.LogEvent(
                EventDataType.EntryUnderTable,
                "Player entered the sitting zone."
            );
            _isInSittingZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SittingZone"))
        {
            UserManager._instance.LogEvent(
                EventDataType.ExitUnderTable,
                "Player exited the sitting zone."
            );
            _isInSittingZone = false;
        }
    }

    public void ItemSelected(SelectEnterEventArgs args)
    {
        UserManager._instance.LogEvent(
            EventDataType.ItemPicked,
            $"Item {args.interactableObject.transform.name} Picked."
        );
    }

    public void ItemDropped(SelectExitEventArgs args)
    {
        UserManager._instance.LogEvent(
            EventDataType.ItemDropped,
            $"Item {args.interactableObject.transform.name} Dropped."
        );
    }

    private void Update()
    {
        if (isSitting && _isInSittingZone)
            return;
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

            _sittingCoroutine = StartCoroutine(
                LerpHeight(isSitting ? defaultHeight : sittingHeight)
            );
            isSitting = !isSitting;
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
