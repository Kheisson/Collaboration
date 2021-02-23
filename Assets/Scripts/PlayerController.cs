using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody rigidbody;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float playerSpeed = 5f;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void OnEnable() => playerControls.Player.Enable();

    private void OnDisable() => playerControls.Player.Disable();

    private void Update()
    {
        MovePlayer();
    }
    public void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void MovePlayer()
    {
        var movementInput = playerControls.Player.Movement.ReadValue<Vector2>();
        var movePosition = new Vector3
        {
            x = movementInput.x,
            z = movementInput.y
        };

        rigidbody.velocity = transform.position + movePosition.normalized * playerSpeed;
    }
}
