using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UserManager : MonoBehaviour
{
    public static UserManager _instance;

    public User CurrentUser;
    private bool inSimulationScene = false;

    private PlayerController _playerController;
    private Transform _playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("There is already an instance of UserManager in the scene.");
            Destroy(this);
            return;
        }
        _instance = this;
        CurrentUser = new User("", "", "");
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Classroom")
        {
            inSimulationScene = true;
            _playerController = FindAnyObjectByType<PlayerController>();
            _playerTransform = _playerController.transform;
        }
        else
        {
            inSimulationScene = false;
        }
        if (scene.name == "MainMenu")
        {
            SaveDataToCSV();
        }
    }

    public void AddeUserInfo(string rollNumber, string ID, string group)
    {
        if (CurrentUser == null)
        {
            CurrentUser = new User(rollNumber, ID, group);
        }
        else
        {
            CurrentUser.RollNumber = rollNumber;
            CurrentUser.ID = ID;
            CurrentUser.Group = group;
        }
    }

    void FixedUpdate()
    {
        if (inSimulationScene)
        {
            CurrentUser.Events.Add(
                new EventData
                {
                    EventPosition = _playerTransform.position,
                    EventRotation = _playerTransform.rotation.eulerAngles,
                    Time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    PlayerSeated = _playerController.isSitting,
                    EventType = EventDataType.RealtimeData,
                    EarthquakeMagnitude = EarthquakeManager._instance.EarthquakeMagnitude,
                }
            );
        }
    }

    public void LogEvent(
        EventDataType eventType,
        string eventDescription,
        Vector3 eventPosition = default,
        Vector3 eventRotation = default
    )
    {
        if (CurrentUser == null)
        {
            Debug.LogError("User not set. Please set the user before logging events.");
            return;
        }
        if (CurrentUser.Events == null)
        {
            CurrentUser.Events = new List<EventData>();
        }
        CurrentUser.Events.Add(
            new EventData
            {
                EventType = eventType,
                EventDescription = eventDescription,
                EventPosition = eventPosition,
                EventRotation = eventRotation,
                Time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            }
        );
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (
            Keyboard.current.escapeKey.wasPressedThisFrame
            && SceneManager.GetActiveScene().name != "MainMenu"
        )
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void SaveDataToCSV()
    {
        if (CurrentUser.RollNumber.IsNullOrEmpty())
            return;

        string directoryPath =
            Application.persistentDataPath + "/UserData" + $"/{CurrentUser.Group}";
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        string path =
            directoryPath + "/" + CurrentUser.RollNumber + "_" + CurrentUser.ID + ".csv";
        Debug.Log(path);
        string csv =
            "Time,Score,Penalty,ID,RollNumber,Group,EventType,EventDescription,EventPosition(X Y Z),EventRotation(X Y Z),EarthquakeMagnitude,PlayerSeated\n";
        csv +=
            $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()},{CurrentUser.Score}, {CurrentUser.Penalty},{CurrentUser.ID},{CurrentUser.RollNumber},{CurrentUser.Group},,,,,,\n";
        foreach (var eventData in CurrentUser.Events)
        {
            csv +=
                $"{eventData.Time},,,,,,{eventData.EventType},{eventData.EventDescription},{$"{eventData.EventPosition.x} {eventData.EventPosition.y} {eventData.EventPosition.z}"},{$"{eventData.EventRotation.x} {eventData.EventRotation.y} {eventData.EventRotation.z}"},{eventData.EarthquakeMagnitude},{eventData.PlayerSeated}\n";
        }
        System.IO.File.WriteAllText(path, csv);
    }
    private string FormatUnixTimestamp(long unixTimestamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        return dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss"); // Format as desired
    }
}
