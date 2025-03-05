using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    UIDocument uiDocument;
    private TextField _IDField,
        _rollNoField;
    private DropdownField _groupField;
    private Button _startSimulationButton,
        _startTrainingButton;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("Main Menu Document not set. Make sure to set it in the inspector.");
            enabled = false;
            return;
        }
        GetUIElements();
        if (UserManager._instance.CurrentUser.ID.IsNotNullOrEmpty())
            _IDField.value = UserManager._instance.CurrentUser.ID.ToString();
        if (UserManager._instance.CurrentUser.RollNumber.IsNotNullOrEmpty())
            _rollNoField.value = UserManager._instance.CurrentUser.RollNumber.ToString();
        if (UserManager._instance.CurrentUser.Group.IsNotNullOrEmpty())
            _groupField.value = UserManager._instance.CurrentUser.Group;
        _startSimulationButton.clicked += StartSimulation;
        _startTrainingButton.clicked += StartTraining;
    }

    private void GetUIElements()
    {
        _IDField = uiDocument.rootVisualElement.Q<TextField>("ID");
        _rollNoField = uiDocument.rootVisualElement.Q<TextField>("RollNumber");
        _groupField = uiDocument.rootVisualElement.Q<DropdownField>("Group");
        _startTrainingButton = uiDocument.rootVisualElement.Q<Button>("PlayTraining");
        _startSimulationButton = uiDocument.rootVisualElement.Q<Button>("PlaySimulation");
    }

    public void StartTraining()
    {
        if (string.IsNullOrEmpty(_IDField.value) || string.IsNullOrEmpty(_rollNoField.value))
        {
            Debug.LogError("Please fill all the fields to start the simulation.");
            return;
        }
        UserManager._instance.AddeUserInfo(_IDField.value, _rollNoField.value, _groupField.value);
        SceneManager.LoadScene("TrainingGround");
    }

    public void StartSimulation()
    {
        if (string.IsNullOrEmpty(_IDField.value) || string.IsNullOrEmpty(_rollNoField.value))
        {
            Debug.LogError("Please fill all the fields to start the simulation.");
            return;
        }
        UserManager._instance.AddeUserInfo(_IDField.value, _rollNoField.value, _groupField.value);
        SceneManager.LoadScene("Classroom");
    }
}
