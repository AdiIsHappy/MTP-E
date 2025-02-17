using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EarthquakeObjectShake : MonoBehaviour
{
    [SerializeField] private float _shakeForceMagnitude = 0.1f;
    [SerializeField] private float noiseMagnitude = 0.1f;
    [SerializeField] private float rotationMagnitude = 0.1f;
    [SerializeField] private float _shakeDuration = 30f;
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
        while (_simulationTime < _shakeDuration)
        {
            Vector3 force = Vector3.zero;
            force += new Vector3(Random.Range(-noiseMagnitude, noiseMagnitude), Random.Range(-noiseMagnitude, noiseMagnitude), Random.Range(-noiseMagnitude, noiseMagnitude));
            _rb.AddForce(force * _shakeForceMagnitude, ForceMode.Acceleration);
            Vector3 torque = new Vector3(Random.Range(-rotationMagnitude, rotationMagnitude), Random.Range(-rotationMagnitude, rotationMagnitude), Random.Range(-rotationMagnitude, rotationMagnitude));
            _rb.AddTorque(torque, ForceMode.Acceleration);
            _simulationTime += Time.deltaTime;
            yield return null;

        }
    }
}

