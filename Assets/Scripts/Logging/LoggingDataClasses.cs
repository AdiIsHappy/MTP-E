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
        RealtimeData = new List<RealtimeData>();
    }

    public string Name { get; set; }
    public string RollNumber { get; set; }
    public string Group { get; set; }
    public int Score { get; set; }
    public int Penalty { get; set; }
    public List<EventData> Events { get; set; }
    public List<RealtimeData> RealtimeData { get; set; }
}

[Serializable]
public class RealtimeData
{
    public SerilizableVector3 position;
    public SerilizableVector3 rotation;
    public bool Seated;
    public float EarthquakeMagnitude;
    public float Time;
}

[Serializable]
public class EventData
{
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
}

public class SerilizableVector3
{
    public float x;
    public float y;
    public float z;

    public SerilizableVector3(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}
