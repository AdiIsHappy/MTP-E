using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    private static readonly int Trigger = Animator.StringToHash("Trigger");
    private static readonly int Grip = Animator.StringToHash("Grip");

    [SerializeField] private Animator animator;
    [SerializeField] private InputActionReference triggerAction;
    [SerializeField] private InputActionReference gripAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (triggerAction == null && gripAction == null && animator == null)
        {
            Debug.Log("Cannot proceed without all components. please attach animator, trigger and grip.");
            enabled = false;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        var triggerValue = triggerAction.action.ReadValue<float>();
        var gripValue = gripAction.action.ReadValue<float>();
        animator.SetFloat(Trigger, triggerValue);
        animator.SetFloat(Grip, gripValue);
    }
}
