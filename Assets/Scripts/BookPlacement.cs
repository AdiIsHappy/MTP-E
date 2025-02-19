using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class BookPlacement : MonoBehaviour
{
    private List<GameObject> PlacedBooks;

    private void Start()
    {
        PlacedBooks = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            PlacedBooks.Add(other.gameObject);
        }

        Debug.Log($"No. Of Books increased to {PlacedBooks.Count}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            if (PlacedBooks.Contains(other.gameObject)) {
                PlacedBooks.Remove(other.gameObject);
        Debug.Log($"No. Of Books decreased to {PlacedBooks.Count}");
            }
        }
    }
}