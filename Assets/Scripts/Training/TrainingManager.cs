using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingManager : MonoBehaviour
{
    private int _booksPlaced = 0;
    private float _typingSpeed = 0.05f;

    [SerializeField]
    private GameObject toolTip;
    private TextMeshProUGUI _toolTipText;

    void Start()
    {
        _toolTipText = toolTip.GetComponentInChildren<TextMeshProUGUI>();
        UpdateToolTip(
            "Welcome to training session, \nMove towards next room using controller.",
            _typingSpeed
        );
        UserManager._instance.LogEvent(EventDataType.TrainingStarted, "Training Started");
    }

    private void OnDestroy()
    {
        UserManager._instance.LogEvent(EventDataType.TrainingEnded, "Training Ended");
    }

    void Update()
    {
        if (_booksPlaced == 3)
        {
            UpdateToolTip(
                "Well done, All books placed. \nAsk researcher to complete the training session.",
                _typingSpeed
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            _booksPlaced++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            _booksPlaced--;
        }
    }

    public void UpdateToolTip(string message, float typingSpeed = 0.05f)
    {
        StartCoroutine(ShowToolTip(message, typingSpeed));
    }

    private IEnumerator ShowToolTip(string message, float typingSpeed)
    {
        toolTip.SetActive(true);
        _toolTipText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            _toolTipText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
