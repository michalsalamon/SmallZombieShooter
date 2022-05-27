using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody))]
public class playerMovmentTest : MonoBehaviour
{
    PlayerControls controls;

    private Rigidbody RB;
    private Vector2 move;
    [SerializeField] private float playerSpeed = 1;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        controls = new PlayerControls();
        //controls.Player.Move.performed += context => SendMessage();
        //controls.Player.Move.performed += ctx => SendMessage(ctx.ReadValue<Vector2>());

        controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void SendMessage()
    {
        Debug.Log("moveButtonPressed");
    }

    void SendMessage(Vector2 coordinates)
    {
        Debug.Log("Move coord: " + coordinates);
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(move.x, 0.0f, move.y);
        RB.AddForce(movement * playerSpeed);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
    }

    private void OnPrimaryActionDown()
    {
        Debug.Log("Primary down");
    }
}
    