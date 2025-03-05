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

    public void AddeUserInfo(string name, string rollNo, string group)
    {
        if (CurrentUser == null)
        {
            CurrentUser = new User(name, rollNo, group);
        }
        else
        {
            CurrentUser.Name = name;
            CurrentUser.RollNumber = rollNo;
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
                    Time = Time.time,
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
                Time = Time.time,
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
        if (CurrentUser.Name.IsNullOrEmpty())
            return;

        string directoryPath =
            Application.persistentDataPath + "/UserData" + $"/{CurrentUser.Group}";
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        string path =
            directoryPath + "/" + CurrentUser.Name + "_" + CurrentUser.RollNumber + ".csv";
        Debug.Log(path);
        string csv =
            "EventType,EventDescription,EventPosition,EventRotation,EarthquakeMagnitude,PlayerSeated,Time,Score,Penalty,Name,RollNumber,Group\n";
        csv +=
            $",,,,,,,{CurrentUser.Score}, {CurrentUser.Penalty},{CurrentUser.Name},{CurrentUser.RollNumber},{CurrentUser.Group}\n";
        foreach (var eventData in CurrentUser.Events)
        {
            csv +=
                $"{eventData.EventType},{eventData.EventDescription},{$"{eventData.EventPosition.x} {eventData.EventPosition.y} {eventData.EventPosition.z}"},{$"{eventData.EventRotation.x} {eventData.EventRotation.y} {eventData.EventRotation.z}"},{eventData.EarthquakeMagnitude},{eventData.PlayerSeated},{eventData.Time}\n";
        }
        System.IO.File.WriteAllText(path, csv);
    }
}
