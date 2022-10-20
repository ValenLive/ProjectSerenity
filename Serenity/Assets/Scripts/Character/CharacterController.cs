using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class CharacterController : MonoBehaviour
{
    private UnityEngine.CharacterController characterController;
    private DefaultInputs defaultInput;

    [SerializeField] private Vector2 inputMovement;
    [SerializeField] private Vector2 inputView;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    [Header("References")]
    [SerializeField] private Transform cameraHolder;

    [Header("Settings")]
    [SerializeField] private PlayerSettingsModel playerSettings;

    [SerializeField] private float viewClampYMin = -70;
    [SerializeField] private float viewClampYMax = 80;

    [Header("Gravity")]
    [SerializeField] private float gravityAmount;
    [SerializeField] private float gravityMin;
    [SerializeField] private float playerGravity;

    private Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    private void Awake()
    {
        defaultInput = new DefaultInputs();

        defaultInput.Character.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e => inputView = e.ReadValue<Vector2>();
        defaultInput.Character.Jump.performed += e => jump();

        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<UnityEngine.CharacterController>();
    }

    private void Update()
    {
        calculateView();
        calculateMovement();
        calculateJump();
    }

    private void calculateView()
    {
        newCharacterRotation.y += playerSettings.viewXSensitivity * (playerSettings.viewXInverted ? -inputView.x : inputView.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);

        newCameraRotation.x += playerSettings.viewYSensitivity * (playerSettings.viewYInverted ? inputView.y : -inputView.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void calculateMovement()
    {
        var verticalSpeed = playerSettings.walkingForwardSpeed * inputMovement.y * Time.deltaTime;
        var horizontalSpeed = playerSettings.walkingStrafeSpeed * inputMovement.x * Time.deltaTime;

        var newMovementSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);

        newMovementSpeed = transform.TransformDirection(newMovementSpeed);

        if (playerGravity > gravityMin && jumpingForce.y < 0.1f)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }

        if (playerGravity < -1 && characterController.isGrounded)
        {
            playerGravity = -1;
        }

        if(jumpingForce.y > 0.1f)
        {
            playerGravity = 0;
        }

        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }

    private void calculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.jumpingFalloff);
    }

    private void jump()
    {
        if (!characterController.isGrounded)
        {
            return;
        }
        Debug.Log("Jump pressed");
        jumpingForce = Vector3.up * playerSettings.jumpingHeight;
    }
}
