using UnityEngine;

public class UpdateTrainingToolTip : MonoBehaviour
{
    public TrainingManager trainingManager;
    public string message;

    void OnTriggerEnter(Collider other)
    {
        trainingManager.UpdateToolTip(message);
    }
}
