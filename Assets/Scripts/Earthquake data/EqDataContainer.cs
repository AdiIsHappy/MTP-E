using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EqDataContainer", menuName = "ScriptableObjects/EqDataContainer", order = 1)]
public class EqDataContainer : ScriptableObject
{
    public float timePeriod = 1.0f;
    public List<EarthquakeData> earthquakeDataList = new List<EarthquakeData>();
}
