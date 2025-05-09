using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int penalty = 0;
    private Dictionary<string, int> _scoreDictionary = new Dictionary<string, int>();
    private Dictionary<string, int> _penaltyDictionary = new Dictionary<string, int>();
    private List<IXRInteractable> _pickedItems = new List<IXRInteractable>();

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
        ObervableItem.ItemObserved += OnItemObserved;
    }

    void Start()
    {
        UpdateScoresAndPenalties();
    }

    private void UpdateScoresAndPenalties()
    {
        _penaltyDictionary.Add("FallingLight", 3);
        _penaltyDictionary.Add("BookRemoved", 3);
        _scoreDictionary.Add("UnderTableSitting", 10);
        _scoreDictionary.Add("BookPlaced", 5);
        _scoreDictionary.Add("ItemObserved", 2);
    }

    private void OnBookPlaced()
    {
        if (
            UserManager._instance.CurrentUser.Group == "Group 1"
            || UserManager._instance.CurrentUser.Group == "Group 2"
        )
            score += _scoreDictionary["BookPlaced"];
        UserManager._instance.CurrentUser.Score = score;
    }

    private void OnBookRemoved()
    {
        if (
            UserManager._instance.CurrentUser.Group == "Group 1"
            || UserManager._instance.CurrentUser.Group == "Group 2"
        )
            penalty += _penaltyDictionary["BookRemoved"];
        UserManager._instance.CurrentUser.Penalty = penalty;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FallingLamp"))
        {
            penalty += _penaltyDictionary["FallingLight"];
            UserManager._instance.CurrentUser.Penalty = penalty;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SittingZone"))
        {
            score += _scoreDictionary["UnderTableSitting"];
            _scoreDictionary["UnderTableSitting"] = 0;
            UserManager._instance.CurrentUser.Score = score;
        }
    }

    void OnItemObserved(GameObject observedItem)
    {
        if (
            UserManager._instance.CurrentUser.Group == "Group 3"
            || UserManager._instance.CurrentUser.Group == "Group 4"
        )
        {
            score += _scoreDictionary["ItemObserved"];
            Debug.Log(observedItem.name);
        }
        
        
    }
}
