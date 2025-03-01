using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSVToEqDataScriptableObject
{
    [MenuItem("Tools/Create Earthquake Data")]
    public static void CreateEarthquakeData()
    {
        string csvPath = EditorUtility.OpenFilePanel(
            "Select CSV File",
            Application.dataPath,
            "csv"
        );

        if (string.IsNullOrEmpty(csvPath))
        {
            return; // User cancelled
        }

        string savePath = EditorUtility.SaveFilePanel(
            "Save Earthquake Data",
            "Assets/data",
            "EqDataContainer",
            "asset"
        );

        if (string.IsNullOrEmpty(savePath))
        {
            return; // User cancelled
        }

        savePath = savePath.Replace(Application.dataPath, "Assets");

        // Read the CSV data
        string[] data = File.ReadAllLines(csvPath); // Use File.ReadAllLines for simpler reading

        // Create a new instance of EqDataContainer
        EqDataContainer eqDataContainer = ScriptableObject.CreateInstance<EqDataContainer>();

        // Parse the data and add to the list
        for (int i = 1; i < data.Length; i++) // Skip the header row
        {
            if (string.IsNullOrEmpty(data[i]))
                continue;

            string[] row = data[i].Split(',');

            //Robust parsing with error handling
            if (row.Length < 3)
            {
                Debug.LogError($"Invalid row format at line {i + 1}. Skipping.");
                continue;
            }

            if (float.TryParse(row[1], out float xAcc) && float.TryParse(row[2], out float yAcc))
            {
                EarthquakeData eqData = new EarthquakeData
                {
                    time = row[0],
                    xAcc = xAcc,
                    yAcc = yAcc,
                };
                eqDataContainer.earthquakeDataList.Add(eqData);
            }
            else
            {
                Debug.LogError($"Error parsing float values at line {i + 1}. Skipping.");
            }
        }

        // Caluculate the frequency from, difference of timestamp of first and second row
        if (eqDataContainer.earthquakeDataList.Count > 1)
        {
            float time1 = float.Parse(eqDataContainer.earthquakeDataList[0].time);
            float time2 = float.Parse(eqDataContainer.earthquakeDataList[1].time);
            eqDataContainer.timePeriod = time2 - time1;
        }

        // Save the ScriptableObject
        AssetDatabase.CreateAsset(eqDataContainer, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", "Earthquake data created successfully!", "OK");
    }
}
