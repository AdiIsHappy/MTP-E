using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingObject : MonoBehaviour
{
    [SerializeField]
    [UnityEngine.Range(0, 1)]
    private double fallingProbabilityMultiplier = 0.1f;

    private List<Joint> _jointsToBreak;
    private EarthquakeManager _earthquakeManager;

    [FormerlySerializedAs("_fallingProbability")]
    [SerializeField]
    private double fallingProbability;

    [SerializeField]
    private float completeFallingTime = 2f;

    private Coroutine _probabiltyUpdater;

    private void Start()
    {
        _earthquakeManager = EarthquakeManager._instance;
        if (_earthquakeManager == null)
        {
            Debug.LogError(
                "No earthquake manager found. cannot proceed without earthquake manager."
            );
            enabled = false;
        }

        _jointsToBreak = gameObject.GetComponents<Joint>().ToList();
        _probabiltyUpdater = StartCoroutine(IUpdateProbaility());
    }

    private void Fall()
    {
        foreach (var joint in _jointsToBreak)
        {
            if (joint == null)
                return;
            joint.breakForce = 0;
            joint.breakTorque = 0;
        }
        gameObject.TryGetComponent<EarthquakeObjectShake>(out var shake);
        if (shake != null)
        {
            shake.objectType = ObjectType.Grounded;
        }
        UserManager._instance.LogEvent(
            EventDataType.FallingLight,
            $"Light fell : {gameObject.name}",
            transform.position,
            transform.rotation.eulerAngles
        );
        Invoke(nameof(CompleteFall), completeFallingTime);
    }

    private void CompleteFall()
    {
        StopCoroutine(_probabiltyUpdater);
        enabled = false;
        gameObject.tag = "Untagged";
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.tag = "Untagged";
            UserManager._instance.LogEvent(
                EventDataType.PlayerHitByFallingLight,
                $"Player hit by falling light : {gameObject.name}",
                transform.position,
                transform.rotation.eulerAngles
            );
        }
    }

    private IEnumerator IUpdateProbaility()
    {
        while (true)
        {
            fallingProbability =
                _earthquakeManager.EarthquakeMagnitude * fallingProbabilityMultiplier;
            if (UnityEngine.Random.value < fallingProbability)
            {
                Fall();
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
