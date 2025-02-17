using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class BookPlacement : MonoBehaviour
{
    private List<GameObject> PlacedBooks;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            PlacedBooks.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            if (PlacedBooks.Contains(other.gameObject)) {
                PlacedBooks.Remove(other.gameObject);
            }
        }
    }
}