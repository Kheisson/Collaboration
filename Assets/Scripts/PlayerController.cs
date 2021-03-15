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
    [SerializeField]
    private int jumpCount;

    private enum PlayerState { onGround, climbing};
    private PlayerState playerState;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerState = PlayerState.onGround;
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
        var moveVector = Vector3.zero;

        if (playerState == PlayerState.climbing)
        {
            moveVector = new Vector3 { x = input.x, y = input.y };
            transform.position += moveVector * playerSpeed * Time.deltaTime;
        }
        else if (playerState == PlayerState.onGround)
        {
            moveVector = new Vector3 { x = input.x, z = input.y };
            transform.position += moveVector * playerSpeed * Time.deltaTime;
            //Handle Rotation
            transform.LookAt(transform.position + moveVector);
        }

        //Handle Walking animation
        if (input.magnitude > 0.1)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

    }
    private void Jumping(InputAction.CallbackContext ctx)
    {
        if (jumpCount < 2)
        {
            animator.SetTrigger("Jump");
            rbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        jumpCount = 0;

        if (collision.gameObject.CompareTag("Climbable"))
        {
            playerState = PlayerState.climbing;
            rbody.useGravity = false;
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            playerState = PlayerState.onGround;
            rbody.useGravity = true;
        }
    }

}
