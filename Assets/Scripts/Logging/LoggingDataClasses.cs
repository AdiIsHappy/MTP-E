using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
    public User(string name, string rollNo, string group)
    {
        Name = name;
        RollNumber = rollNo;
        Group = group;
        Score = 0;
        Penalty = 0;
        Events = new List<EventData>();
    }

    public string Name { get; set; }
    public string RollNumber { get; set; }
    public string Group { get; set; }
    public int Score { get; set; }
    public int Penalty { get; set; }
    public List<EventData> Events { get; set; }
}

// [Serializable]
// public class RealtimeData
// {
//     public Vector3 position;
//     public Vector3 rotation;
//     public bool Seated;
//     public float EarthquakeMagnitude;
//     public float Time;
// }

[Serializable]
public class EventData
{
    public Vector3 EventPosition;
    public Vector3 EventRotation;
    public float EarthquakeMagnitude;
    public bool PlayerSeated;
    public EventDataType EventType { get; set; }
    public string EventDescription { get; set; }
    public float Time { get; set; }
}

[Serializable]
public enum EventDataType
{
    BookPlaced,
    BookRemoved,
    FallingLight,
    EntryUnderTable,
    ExitUnderTable,
    ItemPicked,
    ItemDropped,
    EarthquakeStart,
    EarthquakeEnd,
    PlayerHitByFallingLight,
    LightFallen,
    RealtimeData,
}
