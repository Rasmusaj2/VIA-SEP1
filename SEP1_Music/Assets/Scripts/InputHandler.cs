using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference leftAction;
    public InputActionReference rightAction;
    public InputActionReference upAction;
    public InputActionReference downAction;

    [Header("Parameters")]
    public double delayCompensationMilliseconds = 0.0;

    [Header("References")]
    public BeatmapController beatmapController;

    private List<InputActionReference> actionReferences;

    private void EnableInput()
    {
        foreach (var actionReference in actionReferences)
        {
            actionReference.action.Enable();
        }
    }

    private void DisableInput()
    {
        foreach (var actionReference in actionReferences)
        {
            actionReference.action.Disable();
        }
    }

    private void OnInputEvent(InputAction.CallbackContext context)
    {
        // To differentiate the actions, loop through the list of actions
        // until we find the one that was performed, which will be the i-th action
        for (int i = 0; i < actionReferences.Count; i++)
        {
            if (context.action != actionReferences[i].action) continue;

            Lane lane = (Lane)i;
            HitPhase phase = (HitPhase)(context.phase - InputActionPhase.Performed); // (Performed = 3, Canceled = 4) -> (0, 1)
            double delayCompensationSeconds = 0.001 * delayCompensationMilliseconds;
            double time = context.time - delayCompensationSeconds;

            beatmapController.Hit(lane, phase, time);
        }
    }

    void OnEnable()
    {
        EnableInput();
    }

    void OnDisable()
    {
        DisableInput();
    }

    void Awake()
    {
        // Put actions into a list for easier access. These are kept in order
        // corresponding to the lane that they are associated with
        actionReferences = new List<InputActionReference>()
        {
            leftAction, rightAction, upAction, downAction,
        };
    }

    void Start()
    {
        // Bind the input callbacks. The same callback is bound to all actions
        foreach (var actionReference in actionReferences)
        {
            actionReference.action.performed += OnInputEvent;
            actionReference.action.canceled += OnInputEvent;
        }
    }
}