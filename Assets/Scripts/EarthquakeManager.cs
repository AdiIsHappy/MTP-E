using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class EarthquakeManager : MonoBehaviour
{
    public static EarthquakeManager _instance;

    [SerializeField]
    private float _earthquakeDuration = 120f;

    [SerializeField]
    private AnimationCurve _earthquakeIntensityCurve;

    [SerializeField]
    float earthquakeStartAfter = 120f;

    [SerializeField] private float endSimulationAfterEndOfEarthquake = 30f;
    private float _timer = 0f;
    private bool earthquakeHappend = false;

    public float EarthquakeMagnitude { get; private set; } = 0f;
    public bool IsEarthquakeHappening { get; private set; } = false;

    private AudioSource _earthquakeAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("There is already an instance of EarthquakeManager in the scene.");
            Destroy(this);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        _earthquakeAudioSource = GetComponent<AudioSource>();
        UserManager._instance.LogEvent(EventDataType.SimulationStarted, "Earthquake Simulation Started");
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= earthquakeStartAfter && !earthquakeHappend)
        {
            _timer = 0f;
            earthquakeHappend = true;
            StartCoroutine(Earthquake());
            UserManager._instance.LogEvent(EventDataType.EarthquakeStart, "Earthquake started.");
        }

        if (_timer >= (earthquakeStartAfter + _earthquakeDuration + endSimulationAfterEndOfEarthquake))
        {
            UserManager._instance.LogEvent(EventDataType.SimulationEnded, "Earthquake Simulation ended.");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public IEnumerator Earthquake()
    {
        float startTime = Time.time;
        IsEarthquakeHappening = true;
        _earthquakeAudioSource.Play();
        while (Time.time - startTime < _earthquakeDuration)
        {
            float intensity = _earthquakeIntensityCurve.Evaluate(
                (Time.time - startTime) / _earthquakeDuration
            );
            EarthquakeMagnitude = intensity;
            _earthquakeAudioSource.volume = intensity;
            yield return null;
        }
        EarthquakeMagnitude = 0f;
        IsEarthquakeHappening = false;
        _earthquakeAudioSource.Stop();
        UserManager._instance.LogEvent(EventDataType.EarthquakeEnd, "Earthquake ended.");
    }
}
