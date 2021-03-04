using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator animator;
    private Rigidbody rbody;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float playerSpeed = 5f;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += Jumping;
    }

    private void OnDisable() => playerControls.Player.Disable();

    private void Update()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        //Handle Movement
        var input = playerControls.Player.Movement.ReadValue<Vector2>();
        var moveVector = new Vector3
        {
            x = input.x,
            z = input.y
        };

        //Handle Walking animation
        if (input.magnitude > 0.1)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

        transform.position += moveVector * playerSpeed * Time.deltaTime;

        //Handle Rotation
        transform.LookAt(transform.position + moveVector);
    }
    private void Jumping(InputAction.CallbackContext ctx)
    {
        animator.SetTrigger("Jump");
        rbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

}
