using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField]
    [UnityEngine.Range(0, 1)]
    private double fallingProbabilityMultiplier = 0.1f;

    private List<Joint> jointsToBreak;
    private EarthquakeManager _earthquakeManager;

    [SerializeField]
    private double _fallingProbability;

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

        jointsToBreak = gameObject.GetComponents<Joint>().ToList();
        _probabiltyUpdater = StartCoroutine(IUpdateProbaility());
    }

    private void Fall()
    {
        foreach (var joint in jointsToBreak)
        {
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
            $"Light fell : {gameObject.name}"
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
                $"Player hit by falling light : {gameObject.name}"
            );
        }
    }

    private IEnumerator IUpdateProbaility()
    {
        while (true)
        {
            _fallingProbability =
                _earthquakeManager.EarthquakeMagnitude * fallingProbabilityMultiplier;
            if (UnityEngine.Random.value < _fallingProbability)
            {
                Fall();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
