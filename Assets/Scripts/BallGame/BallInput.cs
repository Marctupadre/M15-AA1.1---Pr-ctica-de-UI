using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallController))]
public class BallInput : MonoBehaviour
{
    BallController controller;
    BallControls input;

    Vector2 moveInput;
    bool jumpPressed;

    void Awake()
    {
        input = new BallControls();

        input.Ball.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Ball.Move.canceled += ctx => moveInput = Vector2.zero;

        input.Ball.Jump.performed += ctx => jumpPressed = true;
    }
    private void Start()
    {
        controller = GetComponent<BallController>();
    }
    void Update()
    {
        controller.Move(moveInput);

        if (jumpPressed)
        {
            controller.Jump();
            jumpPressed = false;
        }
    }

    void OnEnable() => input.Enable();

    void OnDisable() => input.Disable();
}
