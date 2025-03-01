using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class EarthquakeObjectShake : MonoBehaviour
{
    public ObjectType objectType;

    [Header("Shake Settings")]
    [FormerlySerializedAs("_shakeForceMagnitude")]
    [SerializeField]
    private float shakeForceMagnitude = 2f;

    [SerializeField]
    private float rotationTorqueMagnitude = 10f;

    [SerializeField]
    public Transform forceApplicationPoint;

    private Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private EarthquakeManager _earthquakeManager;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _earthquakeManager = FindAnyObjectByType<EarthquakeManager>();
        if (_earthquakeManager == null)
        {
            Debug.LogError("EarthquakeManager not found in the scene.");
            enabled = false;
            return;
        }

        if (objectType == ObjectType.Hanging && forceApplicationPoint == null)
        {
            Debug.LogError(
                "Force Application Point not set. Make sure to set it in the inspector."
            );
            enabled = false;
            return;
        }
    }

    private void FixedUpdate()
    {
        if (!_earthquakeManager.IsEarthquakeHappening)
            return;
        Vector3 force = Vector3.zero;
        force += new Vector3(
            Random.Range(-shakeForceMagnitude, shakeForceMagnitude),
            Random.Range(-shakeForceMagnitude, shakeForceMagnitude),
            Random.Range(-shakeForceMagnitude, shakeForceMagnitude)
        );
        if (objectType == ObjectType.Grounded)
        {
            _rb.AddForce(force * _earthquakeManager.EarthquakeMagnitude, ForceMode.Acceleration);
            Vector3 torque = new Vector3(
                Random.Range(-rotationTorqueMagnitude, rotationTorqueMagnitude),
                Random.Range(-rotationTorqueMagnitude, rotationTorqueMagnitude),
                Random.Range(-rotationTorqueMagnitude, rotationTorqueMagnitude)
            );
            _rb.AddTorque(torque, ForceMode.Acceleration);
        }
        else
        {
            _rb.AddForceAtPosition(
                force * _earthquakeManager.EarthquakeMagnitude,
                forceApplicationPoint.position,
                ForceMode.Acceleration
            );
        }
    }
}
