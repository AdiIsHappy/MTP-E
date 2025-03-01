using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EarthquakeManager : MonoBehaviour
{
    public static EarthquakeManager _instance;

    [SerializeField]
    private float _earthquakeDuration = 120f;

    [SerializeField]
    private AnimationCurve _earthquakeIntensityCurve;

    [SerializeField]
    float earthquakeStartAfter = 120f;
    private float _timer = 0f;
    private bool earthquakeHappend = false;

    public float EarthquakeMagnitude { get; private set; } = 0f;
    public bool IsEarthquakeHappening { get; private set; } = false;

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

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= earthquakeStartAfter && !earthquakeHappend)
        {
            _timer = 0f;
            StartCoroutine(Earthquake());
            UserManager._instance.LogEvent(EventDataType.EarthquakeStart, "Earthquake started.");
            earthquakeHappend = true;
        }
    }

    public IEnumerator Earthquake()
    {
        float startTime = Time.time;
        IsEarthquakeHappening = true;
        while (Time.time - startTime < _earthquakeDuration)
        {
            float intensity = _earthquakeIntensityCurve.Evaluate(
                (Time.time - startTime) / _earthquakeDuration
            );
            EarthquakeMagnitude = intensity;
            yield return null;
        }
        EarthquakeMagnitude = 0f;
        IsEarthquakeHappening = false;
        UserManager._instance.LogEvent(EventDataType.EarthquakeEnd, "Earthquake ended.");
    }
}
