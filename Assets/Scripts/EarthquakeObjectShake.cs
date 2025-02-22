using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class EarthquakeObjectShake : MonoBehaviour
{
    [FormerlySerializedAs("_shakeForceMagnitude")] [SerializeField] private float shakeForceMagnitude = 0.1f;
    [SerializeField] private float noiseMagnitude = 0.1f;
    [SerializeField] private float rotationMagnitude = 0.1f;
    [FormerlySerializedAs("_shakeDuration")] [SerializeField] private float shakeDuration = 30f;
    private Rigidbody _rb;
    private float _simulationTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartShake();
    }

    public void StartShake()
    {
        StartCoroutine(IShake());
    }

    public IEnumerator IShake()

    {
        while (_simulationTime < shakeDuration)
        {
            Vector3 force = Vector3.zero;
            force += new Vector3(Random.Range(-noiseMagnitude, noiseMagnitude), Random.Range(-noiseMagnitude, noiseMagnitude), Random.Range(-noiseMagnitude, noiseMagnitude));
            _rb.AddForce(force * shakeForceMagnitude, ForceMode.Acceleration);
            Vector3 torque = new Vector3(Random.Range(-rotationMagnitude, rotationMagnitude), Random.Range(-rotationMagnitude, rotationMagnitude), Random.Range(-rotationMagnitude, rotationMagnitude));
            _rb.AddTorque(torque, ForceMode.Acceleration);
            _simulationTime += Time.deltaTime;
            yield return null;

        }
    }
}

