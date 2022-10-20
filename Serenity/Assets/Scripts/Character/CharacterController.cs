using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private DefaultInputs defaultInput;

    [SerializeField] private Vector2 inputMovement;
    [SerializeField] private Vector2 inputView;

    private void Awake()
    {
        defaultInput = new DefaultInputs();

        defaultInput.Character.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => inputView = e.ReadValue<Vector2>();

        defaultInput.Enable();
    }
}
