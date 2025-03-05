using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int penalty = 0;
    public Dictionary<string, int> scoreDictionary = new Dictionary<string, int>();
    public Dictionary<string, int> penaltyDictionary = new Dictionary<string, int>();

    [SerializeField]
    private BookPlacement bookPlacement;

    void OnEnable()
    {
        bookPlacement.BookPlaced.AddListener(OnBookPlaced);
        bookPlacement.BookRemoved.AddListener(OnBookRemoved);
    }

    void OnDisable()
    {
        bookPlacement.BookPlaced.RemoveListener(OnBookPlaced);
        bookPlacement.BookRemoved.RemoveListener(OnBookRemoved);
    }

    void Start()
    {
        UpdateScoresAndPenalties();
    }

    private void UpdateScoresAndPenalties()
    {
        penaltyDictionary.Add("FallingLight", 3);
        penaltyDictionary.Add("BookRemoved", 3);
        scoreDictionary.Add("UnderTableSitting", 10);
        scoreDictionary.Add("BookPlaced", 5);
    }

    private void OnBookPlaced()
    {
        score += scoreDictionary["BookPlaced"];
        UserManager._instance.CurrentUser.Score = score;
    }

    private void OnBookRemoved()
    {
        penalty += penaltyDictionary["BookRemoved"];
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FallingLamp"))
        {
            penalty += penaltyDictionary["FallingLight"];
            UserManager._instance.CurrentUser.Penalty = penalty;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SittingZone"))
        {
            score += scoreDictionary["UnderTableSitting"];
            scoreDictionary["UnderTableSitting"] = 0;
            UserManager._instance.CurrentUser.Score = score;
        }
    }
}
