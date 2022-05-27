using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  


[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(HandController))]
[RequireComponent(typeof(InteractionsController))]

public class Player : MainHealth
{
    private PlayerController playerController;
    private PlayerControls controls;
    private HandController handController;
    private InteractionsController interactionsController;

    //player controller variables
    private int verticalAxisSwitchModifier = -1;

    //input variables
    private Vector2 move;
    private float playerMaxSpeed;

    //player characteristicks
    [SerializeField] private float playerAcceleration = 500;
    [SerializeField] private float playerWalkSpeed = 2;
    [SerializeField] private float playerRunSpeedModifier = 2;
    [SerializeField][Range (0, 1)] private float playerAirControl = 0.5f;
    [SerializeField] private float playerLookSpeed = 200;
    [SerializeField] private float playerJumpForce = 40;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        controls = new PlayerControls();
        handController = FindObjectOfType<HandController>();
        interactionsController = FindObjectOfType<InteractionsController>();

        playerMaxSpeed = playerWalkSpeed;

        //input system
        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
        controls.Player.LookArond.performed += ctx => playerController.LookAround(ctx.ReadValue<Vector2>(), playerLookSpeed, verticalAxisSwitchModifier);

        controls.Player.Jump.performed += ctx => playerController.ActorJump(playerJumpForce);
        controls.Player.Crouch.performed += ctx => playerController.ActorCrouch();
        controls.Player.Crouch.canceled += ctx => playerController.ActorStandUp();
        controls.Player.Sprint.performed += ctx => playerMaxSpeed = playerWalkSpeed * playerRunSpeedModifier;
        controls.Player.Sprint.canceled += ctx => playerMaxSpeed = playerWalkSpeed;

        controls.Player.PrimaryAction.performed += ctx => handController.UseItemPrimary();
        controls.Player.PrimaryAction.canceled += ctx => handController.StopUsingItemPrimary();
        controls.Player.SecondaryAction.performed += ctx => handController.UseItemSecondary();
        controls.Player.SecondaryAction.canceled += ctx => handController.StopUsingItemSecondary();

        controls.Player.Interact.performed += ctx => interactionsController.ActorInteracting();

        controls.Player.DropItem.performed += ctx => handController.DropItemInHand();
    }

    //for input system
    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown((KeyCode)(48 + i)))
            {
                handController.EquipItem(i);
            }
        }
    }

    private void FixedUpdate()
    {
        if (controls.Player.Move.IsPressed())
        {
            playerController.MoveActor(new Vector3(move.x, 0, move.y), playerAcceleration, playerMaxSpeed, playerAirControl);
        }

        handController.AimWithItems();
        interactionsController.IsInteractable();
    }
}