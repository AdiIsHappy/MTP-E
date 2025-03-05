using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookPlacement : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _placedBooks;
    public UnityEvent BookPlaced,
        BookRemoved;

    private void Start()
    {
        if (_placedBooks == null)
            _placedBooks = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Book") && !_placedBooks.Contains(other.gameObject))
        {
            _placedBooks.Add(other.gameObject);
            Debug.Log($"No. Of Books increased to {_placedBooks.Count}");
            BookPlaced?.Invoke();
            other.tag = "Untagged";
            UserManager._instance.LogEvent(
                EventDataType.BookPlaced,
                "Book placed on the shelf.",
                other.gameObject.transform.position,
                other.gameObject.transform.rotation.eulerAngles
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            if (_placedBooks.Contains(other.gameObject))
            {
                _placedBooks.Remove(other.gameObject);
                Debug.Log($"No. Of Books decreased to {_placedBooks.Count}");
                BookRemoved?.Invoke();
                UserManager._instance.LogEvent(
                    EventDataType.BookRemoved,
                    "Book removed from the shelf.",
                    other.gameObject.transform.position,
                    other.gameObject.transform.rotation.eulerAngles
                );
            }
        }
    }
}
