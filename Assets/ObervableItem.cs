using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Utilities;

public class ObervableItem : MonoBehaviour
{
    [SerializeField] private ProgressBarCircle progressBar;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float progress = 0f;
    [SerializeField] private Transform maincamera;
    [SerializeField] private Transform canvasTransform;

    private Coroutine coroutine;
    public static Action<GameObject> ItemObserved;

    private int selectingInteractors = 0; // Track the number of selecting interactors

    void Start()
    {
        if (maincamera == null)
        {
            maincamera = Camera.main.transform;
        }
        canvasTransform.gameObject.SetActive(false);
    }

    public void ObjectSelected()
    {
        selectingInteractors++; // Increment when an interactor selects
        canvasTransform.gameObject.SetActive(true);

        if (progress == 0)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            coroutine = StartCoroutine(ObserveObject());
        }
    }

    public void ObjectDeselected()
    {
        selectingInteractors--; // Decrement when an interactor deselects

        // Only reset progress if NO interactors are still selecting
        if (selectingInteractors <= 0)
        {
            selectingInteractors = 0; // Ensure it doesn't go negative
            canvasTransform.gameObject.SetActive(false);
            if (!Mathf.Approximately(progress, 100f))
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                progress = 0f;
            }
        }
    }

    void Update()
    {
        progressBar.BarValue = 100 - progress;
        canvasTransform.LookAt(maincamera);
    }

    private IEnumerator ObserveObject()
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float elapsedTime = Time.time - startTime;
            float normalizedTime = elapsedTime / duration;
            progress = Mathf.Lerp(0f, 100f, normalizedTime);
            yield return null;
        }

        progress = 100f;
        progressBar.MaskColor = Color.green;
        gameObject.tag = "ProgressCompleted";
        ItemObserved?.Invoke(gameObject);
        if (UserManager._instance != null)
        {
            UserManager._instance.LogEvent(EventDataType.ItemObserved, $"Observed {transform.parent.name}", transform.position, transform.rotation.eulerAngles);
        }
    }
}