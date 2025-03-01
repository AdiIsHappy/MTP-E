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
            SaveDataToJSON();
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
            CurrentUser.RealtimeData.Add(
                new RealtimeData
                {
                    position = new SerilizableVector3(_playerTransform.position),
                    rotation = new SerilizableVector3(_playerTransform.rotation.eulerAngles),
                    Time = Time.time,
                    Seated = _playerController.isSitting,
                    EarthquakeMagnitude = EarthquakeManager._instance.EarthquakeMagnitude,
                }
            );
        }
    }

    public void LogEvent(EventDataType eventType, string eventDescription)
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

    void SaveDataToJSON()
    {
        if (CurrentUser.Name.IsNullOrEmpty())
            return;

        string directoryPath = Application.persistentDataPath + "/UserData";
        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        string path = directoryPath + "/" + CurrentUser.RollNumber + ".json";
        Debug.Log(path);
        string json = JsonConvert.SerializeObject(CurrentUser);
        System.IO.File.WriteAllText(path, json);
    }
}
