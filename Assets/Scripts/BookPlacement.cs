using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class BookPlacement : MonoBehaviour
{
    private List<GameObject> _placedBooks;
    

    private void Start()
    {
        _placedBooks = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            _placedBooks.Add(other.gameObject);
        }

        Debug.Log($"No. Of Books increased to {_placedBooks.Count}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            if (_placedBooks.Contains(other.gameObject)) {
                _placedBooks.Remove(other.gameObject);
        Debug.Log($"No. Of Books decreased to {_placedBooks.Count}");
            }
        }
    }
}