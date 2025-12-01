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

    private void OnInputPressed(InputAction.CallbackContext context)
    {
        for (int i = 0; i < actionReferences.Count; i++)
        {
            if (context.action != actionReferences[i].action) continue;

            Debug.Log($"{i.ToString()} pressed");
        }
    }

    private void OnInputReleased(InputAction.CallbackContext context)
    {
        for (int i = 0; i < actionReferences.Count; i++)
        {
            if (context.action != actionReferences[i].action) continue;

            Debug.Log($"{i.ToString()} released");
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
        actionReferences = new List<InputActionReference>()
        {
            leftAction, rightAction, upAction, downAction,
        };
    }

    void Start()
    {
        foreach (var actionReference in actionReferences)
        {
            actionReference.action.performed += OnInputPressed;
            actionReference.action.canceled += OnInputReleased;
        }
    }
}